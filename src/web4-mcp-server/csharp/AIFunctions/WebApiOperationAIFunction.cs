using AnyRestAPIMCPServer.Models;
using Microsoft.Extensions.AI;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AnyRestAPIMCPServer.AIFunctions
{
	internal sealed class WebApiOperationAIFunction : Microsoft.Extensions.AI.AIFunction
	{
		private readonly HttpClient httpClient;
		private readonly OpenApiOperation operation;
		private readonly OpenApiDocument document;

		private readonly JsonSerializerOptions jsonOptions;
		private readonly JsonElement jsonSchema;

		public WebApiOperationAIFunction(HttpClient httpClient, OpenApiOperation operation, OpenApiDocument document)
		{
			this.httpClient = httpClient;
			this.operation = operation;
			this.document = document;

			this.jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
			this.jsonSchema = GenerateOpenAICallSchema(operation);
		}
		public override string Name => operation.OperationId;

		public override string Description => operation.Description;

		public override IReadOnlyDictionary<string, object?> AdditionalProperties => base.AdditionalProperties;
		public override JsonElement JsonSchema => jsonSchema;
		public override JsonElement? ReturnJsonSchema => base.ReturnJsonSchema;

		public override JsonSerializerOptions JsonSerializerOptions => jsonOptions;

		public override System.Reflection.MethodInfo? UnderlyingMethod => null;

		private static JsonElement GenerateOpenAICallSchema(OpenApiOperation operation)
		{
			JsonObject properties = new JsonObject();
			JsonArray requiredParameters = new JsonArray();

			foreach (var p in operation.Parameters)
			{
				if (p.Required)
				{
					requiredParameters.Add(p.Name);
				}

				properties.Add(p.Name, new JsonObject
				{
					["type"] = MapType(p.Type),
					["description"] = p.Description
				});
			}

			JsonObject parametersObject = new JsonObject();
			parametersObject.Add("type", "object");
			parametersObject.Add("properties", properties);

			if (requiredParameters.Count > 0)
			{
				parametersObject.Add("required", requiredParameters);
			}

			var parametersSchemaString = parametersObject.ToJsonString();

			using var doc = JsonDocument.Parse(parametersSchemaString);
			return doc.RootElement.Clone();
		}

		protected override async ValueTask<object?> InvokeCoreAsync(AIFunctionArguments arguments, CancellationToken cancellationToken)
		{
			var argDict = arguments.ToDictionary();

			// Compose URL
			var path = operation.Path;
			foreach (var p in operation.Parameters.Where(x => x.In == "path"))
			{
				if (argDict.TryGetValue(p.Name, out var v) && v is not null)
				{
					path = path.Replace($"{{{p.Name}}}", Uri.EscapeDataString(v.ToString()!));
				}
			}

			var baseUrl = document?.ServerUrl?.TrimEnd('/');
			var urlBuilder = new System.Text.StringBuilder();
			urlBuilder.Append(baseUrl);
			if (!path.StartsWith("/")) urlBuilder.Append('/');
			urlBuilder.Append(path);

			// Query params
			var queryParams = new List<string>();
			foreach (var qp in operation.Parameters.Where(x => x.In == "query"))
			{
				if (argDict.TryGetValue(qp.Name, out var v) && v is not null)
				{
					queryParams.Add($"{Uri.EscapeDataString(qp.Name)}={Uri.EscapeDataString(v.ToString()!)}");
				}
			}
			if (queryParams.Count > 0)
			{
				urlBuilder.Append('?').Append(string.Join('&', queryParams));
			}

			var request = new HttpRequestMessage(new HttpMethod(operation.HttpMethod), urlBuilder.ToString());

			// Simple body handling: collect non path/query params for non-GET verbs
			if (!string.Equals(operation.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
			{
				var bodyParams = new Dictionary<string, object?>();
				foreach (var bp in operation.Parameters.Where(x => x.In != "path" && x.In != "query"))
				{
					if (argDict.TryGetValue(bp.Name, out var v))
						bodyParams[bp.Name] = v;
				}

				if (bodyParams.Count > 0)
				{
					request.Content = new StringContent(JsonSerializer.Serialize(bodyParams, jsonOptions), System.Text.Encoding.UTF8, "application/json");
				}
			}

			using var response = await httpClient.SendAsync(request, cancellationToken);
			var content = await response.Content.ReadAsStringAsync(cancellationToken);

			return content;
		}

		private static string MapType(string openApiType) => openApiType switch
		{
			"integer" => "number",
			"number" => "number",
			"boolean" => "boolean",
			"array" => "array",
			_ => "string"
		};
	}
}
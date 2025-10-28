using AnyRestAPIMCPServer.Models;
using Microsoft.Extensions.AI;
using System.Text.Json;

namespace AnyRestAPIMCPServer.AIFunctions
{
	internal sealed class WebApiOperationAIFunction : Microsoft.Extensions.AI.AIFunction
	{
		private readonly HttpClient httpClient;
		private readonly OpenApiOperation operation;
		private readonly OpenApiDocument document;
		private readonly JsonSerializerOptions jsonOptions;
		private readonly string baseUrl;

		public WebApiOperationAIFunction(HttpClient httpClient, OpenApiOperation operation, OpenApiDocument document)
		{
			this.httpClient = httpClient;
			this.operation = operation;
			this.document = document;
			this.jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

			baseUrl = document.ServerUrl.TrimEnd('/');
		}

		public override string Description => operation.Description;

		public override JsonSerializerOptions JsonSerializerOptions => jsonOptions;

		public override System.Reflection.MethodInfo? UnderlyingMethod => null;

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

			// Try parse JSON
			try
			{
				using var doc = JsonDocument.Parse(content);
				return doc.RootElement.Clone();
			}
			catch
			{
				// Fallback raw string
				return content;
			}
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
using AnyRestAPIMCPServer.Models;
using System.Reflection;
using System.Text.Json;

namespace AnyRestAPIMCPServer.AIFunction
{
	public class WebApiAIFunction : Microsoft.Extensions.AI.AIFunction
	{
		private readonly OpenApiDocument openApiDocument;

		internal WebApiAIFunction(OpenApiDocument openApiDocument)
		{
			this.openApiDocument = openApiDocument;
		}

		public override MethodInfo? UnderlyingMethod => base.UnderlyingMethod;

		public override JsonSerializerOptions JsonSerializerOptions => base.JsonSerializerOptions;

		protected override ValueTask<object?> InvokeCoreAsync(Microsoft.Extensions.AI.AIFunctionArguments arguments, CancellationToken cancellationToken)
		{
			// Example usage of new fields (not required, but shows availability)
			// var first = openApiDocument.Operations.FirstOrDefault();
			// string? server = openApiDocument.ServerUrl;
			return new ValueTask<object?>("test value 123");
		}
	}
}

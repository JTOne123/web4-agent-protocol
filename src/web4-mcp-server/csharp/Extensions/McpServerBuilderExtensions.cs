using AnyRestAPIMCPServer.AIFunctions;
using Microsoft.Extensions.DependencyInjection;
using Utils;

namespace AnyRestAPIMCPServer.Extensions
{
	internal static class McpServerBuilderExtensions
	{
		public static IMcpServerBuilder WithWebAPITools(this IMcpServerBuilder builder, string swaggerEndpoint)
		{
			try
			{
				var http = new HttpClient();
				var json = http.GetStringAsync(swaggerEndpoint).GetAwaiter().GetResult();
				var doc = OpenApiParser.Parse(json);

				foreach (var op in doc.Operations)
				{
					var fn = new WebApiOperationAIFunction(http, op, doc);
					builder.Services.AddSingleton(_ => ModelContextProtocol.Server.McpServerTool.Create(fn));
				}
			}
			catch
			{
				// swallow for now
			}

			return builder;
		}
	}
}
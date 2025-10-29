using Web4AgentProtocol.OpenAPIMCPServer.AIFunctions;
using Microsoft.Extensions.DependencyInjection;
using Utils;

namespace Web4AgentProtocol.OpenAPIMCPServer.Extensions
{
	internal static class McpServerBuilderExtensions
	{
		public static IMcpServerBuilder WithWebAPITools(this IMcpServerBuilder builder, string swaggerEndpoint)
		{
			try
			{
				var httpClient = new HttpClient();
				var json = httpClient.GetStringAsync(swaggerEndpoint).GetAwaiter().GetResult();
				var doc = OpenApiParser.Parse(json);

				foreach (var op in doc.Operations)
				{
					var fn = new WebApiOperationAIFunction(httpClient, op, doc);
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
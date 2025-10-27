using AnyRestAPIMCPServer.AIFunction;
using Microsoft.Extensions.DependencyInjection;
using Utils;

namespace AnyRestAPIMCPServer.Extentaions
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
				var dynamicFn = new WebApiAIFunction(doc);
				builder.Services.AddSingleton(_ => ModelContextProtocol.Server.McpServerTool.Create(dynamicFn));
			}
			catch
			{
				// swallow for now; could add logging
			}

			return builder;
		}
	}
}

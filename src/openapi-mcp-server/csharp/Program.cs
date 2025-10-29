using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web4AgentProtocol.OpenAPIMCPServer.Extensions;
using Web4AgentProtocol.OpenAPIMCPServer.Tools;
using Web4AgentProtocol.OpenAPIMCPServer.Utils;

if (args.Length == 0)
{
	throw new ArgumentException("This MCP server requires a valid OpenAPI specification URL.");
}

var openApiSpecUrl = args[0];

if (!UrlValidator.IsValidHttpUrl(openApiSpecUrl))
{
	throw new ArgumentException("Invalid OpenAPI specification URL. Must be an absolute http/https URL.", nameof(openApiSpecUrl));
}

var builder = Host.CreateApplicationBuilder(args);

// Configure all logs to go to stderr (stdout is used for the MCP protocol messages).
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

// Add the MCP services: the transport to use (stdio) and the tools to register.
builder.Services
	.AddMcpServer()
	.WithStdioServerTransport()
	.WithTools<RandomNumberTools>()
	.WithWebAPITools(openApiSpecUrl);

await builder.Build().RunAsync();
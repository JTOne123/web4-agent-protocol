namespace Web4AgentProtocol.OpenAPIMCPServer.Models
{
	internal sealed record OpenApiDocument(
		string? ServerUrl,
		IReadOnlyList<OpenApiOperation> Operations);

	internal sealed record OpenApiOperation(
		string OperationId,
		string Description,
		string Path,
		string HttpMethod,
		IReadOnlyList<OpenApiParameter> Parameters,
		string ReturnDescription);

	internal sealed record OpenApiParameter(
		string Name,
		string Description,
		string Type,
		string In,
		bool Required);
}
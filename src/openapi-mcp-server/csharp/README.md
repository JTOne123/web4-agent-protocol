# OpenAPI MCP Server (web4-agent-protocol.openapi-mcp-server)

An open standard for AI agents to act, not just aggregate data. This NuGet package provides an intelligent Model Context Protocol (MCP) gateway that lets any existing REST/OpenAPI service be safely consumed by AI agents (Copilot, ChatGPT, autonomous agent frameworks) without bespoke integration work.

## Pillar 1: MCP Gateway + Definition File
The OpenAPI MCP Server runs as a smart gateway (an `openapo-mcp-server`) in front of a Publisher's existing REST API.
- Endpoint discovery
- Request/response schema mapping
- Validation
- Future monetization / microtransaction rules

Publishers do NOT rewrite business logic. They expose (or adapt) an OpenAPI (Swagger) specification enriched with minimal MCP extensions.

When using `.nupkg` from NuGet, reference it via `dnx` in the client configuration.

## Configuration Example (Client Side)
```json
{
  "servers": {
    "web4-agent-protocol.openapi-mcp-server": {
      "type": "stdio",
      "command": "dnx",
      "args": ["web4-agent-protocol.openapi-mcp-server", "--version", "<version>", "--yes", "--", "https://localhost:7293/openapi/v1.json"]
    }
  }
}
```
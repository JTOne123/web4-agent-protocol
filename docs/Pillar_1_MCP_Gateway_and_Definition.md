# 2. Pillar 1: The MCP Gateway and Definition File

This pillar defines the technical foundation that enables the new user-web experience. It introduces the concept of an intelligent gateway (`mcp-server`), which, guided by a standardized definition file (`mcp.api.definition.json`), provides unified and secure access to any existing REST API. This allows a user's AI Agent to dynamically discover and use the functionality of websites without needing custom integration for each one.

### The `any-rest-api-mcp` Server as an Intelligent Gateway

The core idea is that Publishers do not need to rewrite their existing business logic. Instead, they deploy an **`any-rest-api-mcp` server** as a smart gateway that sits "in front of" their internal API. This gateway dynamically reads the `mcp.api.definition.json` file and, based on it, automatically manages routing, request validation, and security.

### The `mcp.api.definition.json` Discovery File

This file is the "brain" of the MCP gateway.

- **Location:** It must be publicly accessible at a standardized path: `/.well-known/mcp/mcp.api.definition.json`. This allows any `mcp-client` to discover it predictably. Crucially, this public, standardized path also enables search engines (like Google and Bing) to index the API, allowing them to act as a user's `mcp-client` to interact with the website directly.
- **Content:** The file is an **OpenAPI 3.0+** specification, extended with a few custom fields:
  - `x-mcp-target`: Specifies the internal API path to which a validated request should be proxied.
  - `x-mcp-monetization`: A structured object that defines the payment and monetization rules for an endpoint.

#### Ease of Adoption and Compatibility

To maximize ease of integration, **Publishers will not need to create this file from scratch**.

Because the specification is based on the OpenAPI 3.0 industry standard, companies that already have a REST API are very likely to also have a description of it in Swagger (OpenAPI 2.0) or OpenAPI 3.0 format. The integration process is reduced to **adapting the existing file**: adding the custom fields `x-mcp-target` for routing and `x-mcp-monetization` for payment rules. Existing tools can be used to automatically convert legacy Swagger 2.0 files to the OpenAPI 3.0 format.

> **Next:** See the [Implementation (Proof of Concepts)](/docs/Implementation_POC.md) for a practical example of building this gateway.
>
> **Next:** See the [Full Example Definition File](/docs/Full_Example_Definition_File.md) for a complete example.

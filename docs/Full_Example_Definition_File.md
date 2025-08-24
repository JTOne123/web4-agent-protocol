# 6. Full Example: `mcp.api.definition.json`

This file is the declarative "brain" of an `mcp-server`. It tells the gateway what endpoints to expose, how to validate them, how to secure them, where to forward them, and how to monetize them.

Below is a complete example for a hypothetical news site, `news-portal.com`, demonstrating all key concepts of the Web4 Agent Protocol.

### `spec/mcp.api.definition.json`
```json
{
  "openapi": "3.0.1",
  "info": {
    "title": "News Portal MCP API",
    "version": "1.0.0",
    "description": "API for accessing articles and user profiles on News Portal, compatible with the Web4 Agent Protocol."
  },
  "servers": [{ "url": "/mcp-api/v1" }],
  "paths": {
    "/articles/{articleId}": {
      "get": {
        "summary": "Get a full article (requires payment or rewarded ad)",
        "operationId": "getFullArticle",
        "parameters": [
          { "name": "articleId", "in": "path", "required": true, "schema": { "type": "string" } }
        ],
        "responses": { 
          "200": { "description": "The full text of the article" },
          "402": { "description": "Payment Required" }
        },
        "x-mcp-target": "/internal/api/articles/{articleId}",
        "x-mcp-monetization": {
          "currency": "USDC",
          "receiverAddress": "0xabc123PublisherEscrowContractAddress",
          "directPayment": {
            "type": "dynamic-invoice",
            "invoiceEndpoint": "/articles/{articleId}/quote"
          },
          "rewardedAd": {
            "pricing": {
              "targetPrice": "0.05",
              "minAcceptablePrice": "0.02"
            }
          }
        }
      }
    },
    "/articles/{articleId}/quote": {
       "post": {
        "summary": "Get an invoice to pay for a full article",
        "operationId": "getArticleQuote",
        "responses": { "200": { "description": "Invoice for payment, valid for 5 minutes" }},
        "x-mcp-target": "/internal/api/payments/invoice"
      }
    },
    "/profile/me": {
      "get": {
        "summary": "Get the current user's profile",
        "operationId": "getProfile",
        "security": [{ "OAuth2Bearer": [] }],
        "responses": { "200": { "description": "User profile data" } },
        "x-mcp-target": "/internal/api/users/me"
      }
    }
  },
  "components": {
    "securitySchemes": {
      "OAuth2Bearer": {
        "type": "http",
        "scheme": "bearer",
        "bearerFormat": "JWT",
        "description": "Standard authorization via OAuth 2.0. The token is obtained after the user logs into their existing account on the website."
      }
    }
  }
}
```
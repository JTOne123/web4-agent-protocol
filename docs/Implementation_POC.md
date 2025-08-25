# Proof-of-Concept (POC) Implementations

This document outlines the structure for various proof-of-concept implementations related to the Web4 Agent protocol. The source code for these POCs are located in the `/src` directory of this project.

## Project Structure

The proposed source code is organized into the following structure under the `/src` directory:

```
/
├── docs/
│   └── articles/
├── src/
│   ├── web4-mcp-client/      # POC: MCP Client
│   ├── web4-mcp-server/      # POC: MCP Server
│   ├── auth-service/         # POC: Authentication Service
│   ├── smart-contracts/      # POC: Smart Contract Infrastructure
│   └── definition-files/     # POC: Example Definition Files
└── ...
```

## 1. POC: `test-web4-mcp-client`

A simple client demonstrating how to interact with a Web4 MCP-enabled server.

### Features:

- **Definition File Support:** Parses a `mcp.api.definition.json` file to understand available API endpoints, authentication requirements, and pricing.
- **Authentication:** Handles authentication flows (e.g., obtaining and using JWTs) to access protected endpoints.
- **On-Chain Payments:** Integrates with a wallet to sign and send on-chain payment transactions as required by the API before making the actual API call.

**Location:** `/src/web4-mcp-client/`

## 2. POC: `web4-mcp-server`

An example of an intelligent gateway server that sits in front of an existing REST API, making it Web4-compatible.

### Features:

- **Dynamic Routing:** Uses a `mcp.api.definition.json` file to configure routing, validation, and pricing.
- **Auth Validation:** Validates incoming requests and authentication tokens (JWTs).
- **Payment Verification:** Verifies on-chain payments before proxying requests to the internal API.

**Location:** `/src/web4-mcp-server/`

## 3. POC: Authentication Service

A standalone service for issuing authentication tokens (JWTs) for the ecosystem.

### Features:

- Issues signed JWTs containing user claims upon successful authentication.

**Location:** `/src/auth-service/`

## 4. POC: Smart Contract Infrastructure

Source code for the smart contracts required for on-chain payments and other potential blockchain interactions.

### Features:

- **Payment Contract:** A simple contract to receive and record payments for API usage.
- **Deployment Scripts:** Scripts for deploying and managing the contracts on a test network.

**Location:** `/src/smart-contracts/`

## 5. POC: Definition File

Examples of `mcp.api.definition.json` files. This file is the "brain" of an MCP server, defining its capabilities, pricing, and security requirements. See a [full example here](./Full_Example_Definition_File.md).

### Features:

- **Endpoint Mapping:** Defines the public-facing API paths and maps them to internal service endpoints.
- **Pricing & Payment:** Specifies the cost for calling an endpoint and the on-chain address for payment.
- **Security Schemes:** Declares the required authentication methods (e.g., OAuth2 Bearer token).

**Location:** `/src/definition-files/`

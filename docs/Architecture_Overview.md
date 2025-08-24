# 1. Architecture Overview

The Web4 Agent Protocol is built on a robust architecture designed for security, scalability, and ease of adoption. Its structure empowers a new, more direct user-web experience, facilitated by AI agents. It is composed of three fundamental pillars that work in concert.

### The Three Pillars of Web4

1.  **[Model Context Protocol (MCP) & Gateway Architecture](/docs/Pillar_1_MCP_Gateway_and_Definition.md):** The technical foundation. This pillar defines how a user's AI Agent can discover and interact with any web service's API in a standardized way.

2.  **[Decentralized Authorization](/docs/Pillar_2_Decentralized_Authorization.md):** The security and trust layer. This pillar ensures that all actions are performed securely on the user's behalf, with the user retaining full control by using their existing website accounts.

3.  **[Native Microtransactions](/docs/Pillar_3_Native_Microtransactions.md):** The economic engine. This pillar provides a flexible and decentralized monetization framework that gives users a choice in how they pay (with funds or with attention) and gives publishers a reliable way to monetize their services.

### Key Participants

The ecosystem revolves around three key roles:

*   **User:** The end-user who directs their `mcp-client` to perform actions. This client could be an LLM chatbot app (e.g., ChatGPT, Google Gemini), a search engine acting as an AI agent, or a dedicated application like the Claude desktop app.
*   **Publisher:** The website or service owner who deploys an `any-rest-api-mcp` server to expose their functionality.
*   **Advertiser:** The entity that funds the user's free access to content in exchange for their verified attention.
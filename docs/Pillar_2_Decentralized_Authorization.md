# 3. Pillar 2: Decentralized Authorization

This pillar solves the challenge of securely performing actions on the user's behalf. Using the industry-standard OAuth 2.0, the protocol allows users to grant their AI Agent access by using their *existing* website accounts. This ensures a high level of security, keeps control in the user's hands, and eliminates the need for them to create and remember new logins and passwords.

### Mechanism and Reuse of Existing Accounts
The protocol’s core principle is **seamless reuse of existing accounts**. When a user's Agent needs to perform an action that requires authorization (e.g., view purchase history), it will initiate a standard OAuth 2.0 flow.

1.  The user is redirected to the Publisher's familiar and trusted login page.
2.  The user authenticates using their existing credentials (e.g., email/password, "Sign in with Google").
3.  After successful login, the user is presented with a consent screen, asking them to grant their Agent specific permissions (scopes), such as `read:orders` or `create:bookings`.
4.  Once consent is given, the `mcp-client` receives an access token it can use for future requests.

### Alternative: Temporary Account Creation
In cases where a user does not have a pre-existing account with a Publisher, the protocol supports the creation of a temporary, single-purpose account. This avoids forcing the user to go through a full sign-up process for a service they may only interact with via their Agent.

This temporary account can be created automatically during the authorization flow. The management of this account, including viewing its status or revoking access, can be handled directly within the `mcp-client` user interface, providing a centralized place for the user to manage all their Agent's connections.

This flow is secure because the user's credentials are never exposed to the `mcp-client` or the LLM. The user is always in control.

### Integration into the Gateway

The `mcp-server` (gateway) enforces this security. In the `mcp.api.definition.json` file, protected endpoints are marked with a `security` requirement. The gateway uses this information to automatically apply a middleware that validates the access token on every incoming request for that endpoint, rejecting any that are unauthorized.

> **Next:** See the [Implementation Guide](/docs/Implementation_Guide_MCP_Server.md) for a code example of how this security is enforced.
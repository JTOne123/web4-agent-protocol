# 5. Implementation Guide: `any-rest-api-mcp` Server

This guide provides a practical, high-level example of how to implement an `any-rest-api-mcp` server as an intelligent gateway using Node.js and Express. The goal is to create a configurable server that can be placed in front of any existing REST API.

### Required Tools
*   **Node.js**
*   **Express:** Web framework.
*   **express-openapi-validator:** Reads an OpenAPI file and automatically handles request validation and routing.
*   **jsonwebtoken:** For handling JWTs for authorization.
*   **http-proxy-middleware:** For proxying requests to the internal API.

### Project Structure
```
mcp-server/
├── index.js              # The main gateway server file
├── api/
│   └── auth.js           # Authorization middleware
├── spec/
│   └── mcp.api.definition.json # The "brain" of the server
└── .env                  # Configuration (secrets, internal API URL)
```

### The `index.js` Gateway Logic

```javascript
// index.js - The main MCP Gateway Server file
require('dotenv').config();
const express = require('express');
const path = require('path');
const { OpenApiValidator } = require('express-openapi-validator');
const { createProxyMiddleware } = require('http-proxy-middleware');
const jwt = require('jsonwebtoken');

const app = express();
const PORT = process.env.PORT || 3000;
const API_SPEC_PATH = path.join(__dirname, 'spec/mcp.api.definition.json');

// Authorization middleware
const checkJwt = (req, res, next) => {
    const authHeader = req.headers.authorization;
    if (!authHeader || !authHeader.startsWith('Bearer ')) {
        return res.status(401).json({ message: 'Missing Authorization header' });
    }
    const token = authHeader.split(' ')[1];
    jwt.verify(token, process.env.JWT_SECRET, (err, decoded) => {
        if (err) return res.status(403).json({ message: 'Invalid token' });
        req.user = decoded; // Make user payload available to the proxy
        next();
    });
};

// Proxy to the Publisher's internal API
const internalApiProxy = createProxyMiddleware({
    target: process.env.INTERNAL_API_URL, // e.g., 'http://localhost:8080'
    changeOrigin: true,
    pathRewrite: (path, req) => {
        // Dynamically get the target path from the validated OpenAPI definition
        return req.openapi.routeDef['x-mcp-target'];
    },
    onProxyReq: (proxyReq, req, res) => {
        // Pass user info to the internal API if available
        if (req.user) proxyReq.setHeader('X-User-Id', req.user.id);
    },
});

app.use(express.json());

// Set up the OpenApiValidator
new OpenApiValidator({
    apiSpec: API_SPEC_PATH,
    validateRequests: true, // Automatically validate requests against the schema
    securityHandlers: { // Link the security scheme name to our middleware
        OAuth2Bearer: checkJwt
    }
}).install(app)
    .then(() => {
        // After validation, all valid API requests are passed to the proxy
        app.use('/mcp-api/v1', internalApiProxy);

        // Standard error handler for validation errors
        app.use((err, req, res, next) => {
            res.status(err.status || 500).json({ message: err.message, errors: err.errors });
        });

        app.listen(PORT, () => {
            console.log(`MCP Gateway Server running on port ${PORT}`);
        });
    });
```
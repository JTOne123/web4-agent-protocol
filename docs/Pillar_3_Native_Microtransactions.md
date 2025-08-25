# 4. Pillar 3: Native Microtransactions & Monetization

This pillar forms the economic engine of the protocol. It introduces a decentralized monetization system based on smart contracts that supports various scenarios. This gives Publishers flexible tools to generate revenue while giving users a transparent choice in how they exchange value for services—either with funds or with their attention.

Leveraging blockchain technology transcends traditional financial barriers. It is not bound by any single country's banking infrastructure or regulations, making it feasible for creators anywhere on the planet to monetize their work and receive fair compensation. The timing is particularly opportune as many countries are beginning to legally adopt stablecoins, providing a stable medium of exchange for these global transactions.

### Smart Contract-Based Infrastructure

The entire financial logic is built on smart contracts to ensure transparency, security, and ease of integration.

- **Contract Factory:** A "factory" for easily creating personal smart contracts for Publishers and Advertisers.
- **`Publisher Escrow Contract`:** The Publisher's personal contract for securely receiving payments.
- **`Advertiser Vault Contract`:** An Advertiser's personal vault contract holding their ad budget.
- **Ad Network Orchestrator:** A decentralized service that acts as a trusted intermediary, managing the ad-based monetization flow.

### Model A: Direct Payment with Proof-of-Purchase

1.  **Payment:** The user makes a blockchain transaction.
2.  **Verification & Token Issuance:** The `mcp-server` verifies the transaction on-chain and, if successful, issues a **short-lived JWT (Proof-of-Purchase Token)** that grants access to the specific resource.
3.  **Content Retrieval:** The user's Agent uses this JWT to make the final request for the content.

### Model B: Dynamic Pricing and Invoicing

For services where the price is not known in advance (e.g., flights, ride-sharing).

1.  **Quote Request:** The Agent requests a precise price for a service.
2.  **Invoice Issuance:** The `mcp-server` returns a unique `invoiceId` and the final amount.
3.  **Payment by Invoice:** The user pays the invoice, embedding the `invoiceId` in the transaction metadata.
4.  A **Proof-of-Purchase Token** is obtained by verifying the invoice payment, which is then used to finalize the booking or purchase.

### Model C: Flexible Direct Unlock (Rewarded Ads)

This is a model that allows users to pay with their attention.

1.  **Flexible Pricing:** The Publisher specifies both a `targetPrice` (the ideal price) and a `minAcceptablePrice`.
2.  **Intelligent Auction:** The Ad Network Orchestrator finds an Advertiser willing to pay the best price for the ad slot within the Publisher's acceptable range.
3.  **Ad View:** The user watches the ad.
4.  **Atomic Operation:** The Orchestrator initiates a payment from the Advertiser to the Publisher and simultaneously issues a cryptographically-signed **`Proof-of-View Voucher`** to the user's Agent.
5.  **Content Unlock:** The Agent uses this voucher to access the content. The Publisher is guaranteed payment without the user ever needing to handle micro-balances.

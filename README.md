# CKO-PaymentGateway - Spec

Note: Technical Readme is under the PaymentGateway dir


# Payment Gateway
A Payment Gateway is responsible for validating requests, storing card information and forwarding
payment requests and accepting payment responses to and from the acquiring bank.

This solution provides a Web API that exposes endpoints to allow users to simulate the above behaviours.
Transaction details are stored in a real database but the bank is simulated.

### Submitting a payment
```mermaid
sequenceDiagram
    participant Merchant
    participant PaymentGateway
    participant bank

    Merchant-)PaymentGateway: HTTP POST paymentRequest
    PaymentGateway->>PaymentGateway: Validate paymentRequest
    alt validation failed
    PaymentGateway->>Merchant: Request Validation Failed
    else validation passed
        PaymentGateway->>bank: HTTP POST processPayment
        bank-->>PaymentGateway: Payment Processed
        PaymentGateway->>PaymentGateway: Store Processing Response
        PaymentGateway-->>Merchant: Payment ID
    end
   ```

### Retrieving a payment
```mermaid
sequenceDiagram
    participant Merchant
    participant PaymentGateway


    Merchant-)PaymentGateway: HTTP GET /GetPayment/<PaymentId>
    PaymentGateway->>PaymentGateway: Validate paymentRequest
    alt validation failed
    PaymentGateway->>Merchant: Request Validation Failed
    else validation passed
        PaymentGateway->>PaymentGateway: Get Payment From DB
        PaymentGateway-->>Merchant: Return Payment Details
    end
   ```
   
#### API Defintions

POST api/MakePayment
{
  "cardNumber": "6330567890123456",
  "cardExpiryMonth": "11",
  "cardExpiryDay": "03",
  "cardCvv": "123",
  "amount": 6760,
  "currency": "GBP"
}

GET api/GetPayment/<PaymentId>
# Payment Gateway Tech Assesment - Summary

## Work Done (Time-boxed 3 hours)
1. Implement APIs
2. Simulate Bank
3. Unit test and Integration tests
4. CI/CD via github actions
5. Add Healthcheck
6. Docker file optimisation
7. Sequence diagram and API docs

## Further work
1. Add more tests
2. Containerised Api and DB. e.g. via docker-compose, to ease deployment
3. Improve exception handling and error reporting (Duplicate payment, retry with bank)
4. Improve health check to include db and bank connection check
5. Add Test Coverage and Static Code Analysis via codeclimate
6. Add Static Application Security Testing via CODEQL
7. Add Tracing and Metrics via 
8. Use Wiremock for better simulation



### Run the Web Api
> This project uses .NET 5 and Docker. It is advisable to have these tools installed on your machine
>Make sure the Docker Daemon is running (Docker Desktop on Windows)

#### Set up the database - run the following in the terminal
1. > docker pull mcr.microsoft.com/mssql/server:2019-latest
2. > docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=2Secure*Password2" -p 1450:1433 --name sqlserverdb -h mysqlserver -d mcr.microsoft.com/mssql/server:2019-latest

Navigate to the WebApi root directory and invoke the dotnet cli

```cmd
cd {REPO}/PaymentGateway/Src/WebApi
dotnet run WebApi
```

### Run tests
Navigate to the WebApi root directory and invoke the dotnet cli

```cmd
cd {REPO}
dotnet tests
```
#### Validate Application is running with healthcheck

Curl or Visit http://localhost:5000/health

#### Making a payment (Example call)
BASE_URL=http://localhost:5000
```bash

curl --location --request POST 'http://localhost:5000/api/MakePayment' \
--header 'Content-Type: application/json' \
--data-raw '{
  "cardNumber": "1234567890123456",
  "cardExpiryMonth": "11",
  "cardExpiryDay": "04",
  "cardCvv": "123",
  "amount": 18,
  "currency": "GBP"
}'

```

#### Retrieving a payment (Example call)
```bash
curl --location --request GET 'http://localhost:5000/api/GetPayment?paymentId=<ID-FROM-POST-CALL>'
```
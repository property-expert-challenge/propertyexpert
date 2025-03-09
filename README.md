# Property Expert API

A .NET 8 Web API for evaluating property repair and maintenance invoices. The system analyzes PDF invoices and provides automated classification and evaluation based on predefined business rules.

## Features

- PDF invoice document processing
- Automated invoice classification
- Rule-based evaluation system
- RESTful API endpoints
- Swagger documentation
- Integration tests
- Input validation using FluentValidation

## Prerequisites

- .NET 8.0 SDK
- An IDE (Visual Studio, VS Code, or JetBrains Rider)

## Project Structure

```
PropertyExpert/
├── WebApi/                 # Main API project
│   ├── Controllers/        # API endpoints
│   ├── Models/            # Data models and DTOs with thier corresponding validators
│   └── Services/          # Business logic and services
└── WebApi.Tests/          # Test project
    ├── Controllers/       # Integration tests
    ├── Services/         # Unit tests
    └── Data/             # Test data files
```

## Getting Started

1. Clone the repository:
```bash
git clone <repository-url>
cd PropertyExpert
```

2. Build the solution:
```bash
dotnet build
```

3. Run the tests:
```bash
dotnet test
```

4. Run the API:
```bash
cd WebApi
dotnet run
```

The API will be available at `https://localhost:5001` with Swagger documentation at `/swagger`.

## API Endpoints

### POST /Evaluation/evaluate

Evaluates a PDF invoice document and returns classification and evaluation results.

#### Request

- Content-Type: `multipart/form-data`
- Body:
  - `Document`: PDF file (required)
  - `Invoice`: Invoice details (required)
    - `InvoiceId`: string
    - `InvoiceNumber`: string (format: S followed by 5 digits)
    - `InvoiceDate`: datetime
    - `Amount`: decimal
    - `Comment`: string (optional)

#### Response

```json
{
  "evaluationId": "EVAL_guid",
  "invoiceId": "string",
  "rulesApplied": ["string"],
  "classification": "string",
  "evaluationFile": "base64string"
}
```

## Configuration

The application uses the following configuration settings (in appsettings.json or environment variables):

```json
{
  "ClassificationApiOptions": {
    "BaseUrl": "https://run.mocky.io/v3/7e114987-f5a5-4a56-8867-575b20ea69c0"
  }
}
```

## Development

### Running Tests

```bash
dotnet test
```

## Security

- PDF file size is limited to 5MB
- Input validation is enforced
- API endpoints use HTTPS
- Sensitive configuration is handled via environment variables


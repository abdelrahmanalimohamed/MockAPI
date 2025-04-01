
# Product API Integration

## Overview

This project is a **RESTful Web API** built with **.NET 8.0**. It extends the functionality of the mock API provided at [https://restful-api.dev](https://restful-api.dev) by adding additional processing and features. 
The API includes methods to interact with the mock API for retrieving, adding, and removing data. It also implements validation for product models and error handling.

## Features

1. **Clean Architecture**: Implements a clean architecture pattern for better maintainability and scalability.
2. **Data Retrieval**: Retrieve data from the mock API with filtering by name (substring) and pagination support.
3. **Add Data**: Add new product data to the mock API.
4. **Remove Data**: Remove existing product data from the mock API.
5. **Validation**: Ensure that the product data is valid before adding or updating.
6. **Error Handling**: Provide clear error messages in case of failure.

## Technologies Used

- **Framework**: .NET 8.0
- **Model Validation**: FluentValidation
- **Error Handling**: Custom exception handling middleware
- **Testing**: XUnit for unit testing
- **Logging**: Structured logging with ILogger

## Setup & Installation

To get the project up and running locally, follow these steps:

### 1. Clone the Repository

```bash
git clone https://github.com/abdelrahmanalimohamed/MockAPI.git
cd MockAPI
```

### 2. Restore Dependencies

Run the following command to restore the necessary NuGet packages:

```bash
dotnet restore
```

### 3. Run the Project

To run the application locally, use the following command:

```bash
dotnet run
```

The API should now be available at `http://localhost:5005`.

### 4. Environment Configuration

If you need to modify settings such as the mock API base URL, you can adjust the `appsettings.json` file:

```json
{
  "ExternalApis": {
  "ProductApiBaseUrl": "https://api.restful-api.dev/objects"
},
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## API Endpoints

### 1. **GET** `/api/products`
Retrieve a list of products from the mock API with the ability to filter by name (substring) and pagination.

#### Query Parameters:
- `search` (optional): A substring to filter product names.
- `page` (optional): The page number for pagination.
- `pageSize` (optional): The number of products per page.

#### Example Request:
```http
GET http://localhost:5000/api/products?search=Sample&page=1&pageSize=10
```

#### Response:
```json
[
          "id": "1",
            "name": "Google Pixel 6 Pro",
            "data": {
                "color": "Cloudy White",
                "capacity": "128 GB",
                "price": null,
                "generation": null,
                "year": null,
                "cpuModel": null,
                "hardDiskSize": null,
                "strapColour": null,
                "caseSize": null,
                "description": null,
                "screenSize": 0
            }
        },
        {
            "id": "2",
            "name": "Apple iPhone 12 Mini, 256GB, Blue",
            "data": null
        },
        {
            "id": "3",
            "name": "Apple iPhone 12 Pro Max",
            "data": {
                "color": "Cloudy White",
                "capacity": null,
                "price": null,
                "generation": null,
                "year": null,
                "cpuModel": null,
                "hardDiskSize": null,
                "strapColour": null,
                "caseSize": null,
                "description": null,
                "screenSize": 0
            }
        },
]
```

### 2. **POST** `/api/products`
Add a new product to the mock API.

#### Request Body:
```json
{
  "name": "New Product",
  "description": "This is a new product",
  "price": 25.99
}
```

#### Example Request:
```http
POST http://localhost:5000/api/products
```

#### Response:
```json
{
  "id": "string",
  "name": "string",
  "data": {
    "color": "string",
    "capacity": "string",
    "price": "string",
    "generation": "string",
    "year": 0,
    "cpuModel": "string",
    "hardDiskSize": "string",
    "strapColour": "string",
    "caseSize": "string",
    "description": "string",
    "screenSize": 0
  }
}
```

### 3. **DELETE** `/api/products/{id}`
Remove a product by ID from the mock API.

#### Example Request:
```http
DELETE http://localhost:5000/api/products/123
```

#### Response:
```json
{
  "message": "Product with ID 123 deleted successfully"
}
```

## Validation

The following validation rules are applied to the product model:

- **Name**: Required, cannot be empty.
- **Description**: Required, cannot be empty.

If any validation fails, a **400 Bad Request** response will be returned with details of the validation errors.

### Example Validation Error Response:
```json
{
  "message": "Validation error",
  "errors": {
    "name": ["Product name is required."],
  }
}
```

## Error Handling

The API includes global exception handling through middleware. If an error occurs, it will return a structured response indicating the error type.

### Example Error Response:
```json
{
  "message": "An error occurred while processing your request."
}
```

For validation errors, a **400 Bad Request** status will be returned with the specific validation error details.

### Example Validation Error:
```json
{
  "message": "Validation error",
  "errors": {
    "name": ["Product name is required."]
  }
}
```

### Example Internal Server Error:
```json
{
  "message": "An unexpected error occurred. Please try again later."
}
```

## Unit Testing

Unit tests are written using **XUnit** and are located in the `tests` directory. To run the unit tests:

### 1. Navigate to the `tests` directory:
cd tests
```

### 2. Run the tests:
dotnet test
```

## Logging

Structured logging is implemented using **ILogger**. Log messages include key events such as validation errors, successful product additions, and internal errors.

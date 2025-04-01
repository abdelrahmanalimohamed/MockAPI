# Project Name: Mock API

## Description
A simple Product API that allows creating and managing products. This API includes validation, exception handling, and basic CRUD operations. Built using ASP.NET Core, MediatR, FluentValidation, and custom exception handling middleware.

## Features
- **CreateProduct**: Allows creating a product with validation for required fields.
-  **UpdateProduct**: Allows updating a product with validation for required fields.
-  **GetProducts**: Allows getting all the products.
-  **DeleteProduct**: Allows deleting a product.
- **ValidationMiddleware**: Ensures input validation using FluentValidation.
- **ExceptionHandling**: Global exception handling middleware to catch and return errors as structured JSON responses.
- **Clean Architecture**: Implements a clean architecture pattern for better maintainability and scalability.

## Technologies Used
- **ASP.NET Core 8**
- **MediatR**: For CQRS pattern implementation.
- **FluentValidation**: For input validation.
- **Logger**: To log error and validation information.
- **Exception Handling Middleware**: Custom middleware to handle and return detailed error responses.
- **XUnit**: For unit testing.

## Setup & Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/MockAPI.git

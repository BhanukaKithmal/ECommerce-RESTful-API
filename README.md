# ECommerce-RESTful-API

A RESTful API built with .NET 8 and Entity Framework Core (Code First) to manage an e-commerce platform. This project supports CRUD operations, pagination, filtering, and unit testing with a MySQL backend.

---

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [Running Unit Tests](#running-unit-tests)
- [Importing Postman Collection](#importing-postman-collection)
- [Contributing](#contributing)

---

## Introduction

This project is a RESTful API designed for managing an e-commerce platform. It provides essential features such as creating, reading, updating, and deleting (CRUD) resources, along with advanced capabilities like pagination and filtering. Unit tests are included to ensure the robustness of the application.

---

## Features

- CRUD operations for managing resources (e.g., products, categories, orders, etc.)
- Pagination and filtering for efficient data retrieval
- MySQL database integration using Entity Framework Core (Code First)
- Built on .NET 8 for modern and efficient development
- Well-structured API endpoints for seamless integration
- Unit testing for ensuring code quality and reliability

---

## Technologies Used

- **Language**: C#
- **Framework**: .NET 8
- **ORM**: Entity Framework Core (Code First)
- **Database**: MySQL
- **Testing**: Unit testing libraries (e.g., xUnit, Moq)
- **API Testing**: Postman
- **Version Control**: Git and GitHub

---

## Installation

### Prerequisites

1. Install [.NET SDK](https://dotnet.microsoft.com/download) (version 8 or later).
2. Install [MySQL Community Server](https://dev.mysql.com/downloads/).
3. Set up a MySQL database for the project.

### Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/BhanukaKithmal/ECommerce-RESTful-API.git
   cd ECommerce-RESTful-API/ECommerceRESTfulAPI
   ```

2. Restore the dependencies:
   ```bash
   dotnet restore
   ```

3. Update the `appsettings.json` or `appsettings.Development.json` file with your MySQL connection string:
   ```json
   "ConnectionStrings": {
      "DefaultConnection": "Server=your_server;Database=your_database;User=your_user;Password=your_password;"
   }
   ```

4. Apply migrations to the database:
   ```bash
   dotnet ef database update
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

---

## Usage

1. Start the API server by running the application.
2. Use tools like [Postman](https://www.postman.com/) or [cURL](https://curl.se/) to interact with the API.
3. Navigate to `https://localhost:{port}/swagger` (default port is 5001) to access the Swagger UI for testing endpoints.

---

## Running Unit Tests

To ensure the quality of the code, unit tests are provided. Follow these steps to run the tests:

1. Navigate to the root directory of the repository.
2. Run the following command:
   ```bash
   dotnet test
   ```

Ensure that any dependencies required for the tests (e.g., test database) are properly configured before running the tests.

---

## Importing Postman Collection

You can use the Postman collection to test the API endpoints. Follow these steps to import it:

1. Download the Postman collection file (`ECommerce-RESTful-API.postman_collection.json`) from the repository. The file should be located in the root directory or a specific folder like `/docs` or `/postman`.
2. Open Postman.
3. Click on the "Import" button in the top-left corner.
4. Select the downloaded Postman collection file and import it.
5. After importing, you can start testing the API endpoints using Postman.

---

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository.
2. Create a branch for your feature or fix:
   ```bash
   git checkout -b feature-name
   ```
3. Commit your changes:
   ```bash
   git commit -m "Description of changes"
   ```
4. Push to your fork and submit a pull request.

---


# Lumberjack API

This project is a .Net 6 Web API that allows you to submit logs in JSON and file formats, which are then stored in a SQL database and sent to an Azure Service Bus. A feature-rich API encompassing logging, authentication, authorization, Swagger documentation, dependency injection, ORM, Azure Service bus, MVC, repository pattern, asynchronous methods, AutoMapper with profiles, and pagination.

## Features

### Logging

The API provides robust logging capabilities, allowing you to capture and store logs in various formats. Logs can be submitted as JSON or file uploads, providing flexibility in how you capture and process log data.

### Authentication and Authorization (JWT Tokens, Claims)

Authentication and authorization are implemented using JSON Web Tokens (JWT). Users can obtain a token by providing their credentials, and subsequent requests to protected endpoints require this token in the Authorization header. Claims can be used to grant or restrict access to certain resources based on user roles or permissions.

### Documentation (Swagger)

The API includes Swagger documentation, which provides a comprehensive and interactive user interface for exploring the available endpoints, their parameters, and responses. Swagger makes it easy to understand and test the API's functionality without requiring external tools.

### Dependency Injection

The project employs the dependency injection pattern to manage and resolve dependencies between classes. This promotes loose coupling, improves testability, and allows for easier extensibility and maintainability of the codebase.

### ORM (Object-Relational Mapping)

An ORM (Object-Relational Mapping) framework, EFCore, is used to interact with the SQL database. This simplifies data access by abstracting away the low-level database operations and allowing developers to work with objects and entities instead.

### MVC (Model-View-Controller)

The MVC pattern promotes a clear separation of concerns and improves code maintainability, testability, and reusability. It provides a structured approach to building Web API applications by organizing code into logical components and enforcing a separation between business logic, data, and presentation.

### Repository Pattern

The repository pattern is implemented to provide a separation between the data access logic and the rest of the application. It encapsulates the data access operations, making it easier to switch between different data sources or storage mechanisms without impacting the higher-level code.

### Asynchronous Methods

The API leverages asynchronous methods to improve performance and scalability. By using asynchronous operations, the server can handle more requests concurrently and efficiently utilize system resources, leading to better responsiveness and scalability.

### AutoMapper (and Profiles)

AutoMapper is utilized to simplify the mapping between different data models and entities. It eliminates the need for manual mapping code, reducing boilerplate and improving development productivity. AutoMapper profiles are used to define specific mappings between source and destination types.

### Pagination

To handle large datasets, pagination is implemented in the API. It allows clients to request a specific subset of results, reducing the amount of data transferred over the network and improving performance. Pagination parameters such as page size and offset can be provided to control the returned results.


## Getting Started

To get started with this project, follow these steps:

1. Clone the repository: `git clone https://github.com/your-username/your-repo.git`
2. Navigate to the project directory: `cd Lumberjack.API`
3. Configure the SQL database connection string and Azure Service Bus settings in the appropriate configuration files.
4. Build and run the project: `dotnet run`


## API Endpoints

The following are the main endpoints provided by the API:

- `POST ​/api​/Authentication​/Authenticate`: Get Security Token
- `GET ​/api​/Messages`: Get all Messages for the application the Security Token has access to
- `GET ​/api​/Messages​/{messageId}`: Get one Messages by Id
- `GET ​/api​/Messages​/{messageId}​/Segments`: Get Segments for a Message by Id
- `GET ​/api​/Messages​/{messageId}​/Segments​/{segmentId}`: Get a Segment by Id

For detailed information on each endpoint, refer to the Swagger documentation provided by the API.


## Contributing

Contributions to this project are welcome. If you encounter any issues or have suggestions for improvements, please open an issue or submit a pull request. Be sure to follow the project's code style and guidelines.


## License

This project is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute the code as per the terms of this license.


## Acknowledgments

We would like to acknowledge the following libraries and frameworks that have been instrumental in the development of this project:

- ASP.NET Core
- Entity Framework Core
- AutoMapper
- Swagger
- JWT (JSON Web Tokens)
- And many other open-source contributions!

Special thanks to the developer community for their support and valuable contributions.



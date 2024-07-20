# Application Functionality

This application is a backend system developed using C# and the ASP.NET Core framework. It is designed to process eligibility files and user registrations. Below are the details on how each part of the application operates.

## Project Structure

The project is divided into several layers, including:

- **Domain/Models**: Contains the data model definitions used in the application.
- **Application/Messages/Commands**: Defines the commands for interacting with the system through the CQRS (Command Query Responsibility Segregation) pattern.
- **Origin.API/Controllers**: Contains the controllers that expose API endpoints for external interaction.

## Data Models

### EligibilityFile

- **File**: Path or content of the eligibility file.
- **EmployerName**: Name of the employer related to the file.

### SignupModel

- **Email**: User's email address.
- **Password**: User's password.
- **Country**: User's country.

## Controllers

### EligibilityFileController

- **Endpoint**: `POST /api/eligibility/process`
- **Description**: Receives an eligibility file and the employer's name, processes this information, and returns a result.
- **Conditions for Calling**: This endpoint is called when there is a new eligibility file to be processed.

### SignupController

- **Endpoint**: `POST /api/signup`
- **Description**: Receives registration information for a new user, such as email, password, and country, and processes the registration.
- **Conditions for Calling**: This endpoint is called when a new user wishes to register in the system.

## Processing

- **ProcessEligibilityFileCommand**: Command sent by `EligibilityFileController` to process the eligibility file.
- **ProcessSignupCommand**: Command sent by `SignupController` to process the registration of a new user.

## Technologies and Patterns Used

- **ASP.NET Core**: Framework for building web applications and APIs.
- **MediatR**: Library for implementing the Mediator pattern, facilitating communication between application components.
- **CQRS**: Pattern used to separate the execution of commands (actions that change state) from queries (actions that return data).

## External Service Endpoints

This section describes the external service endpoints that the application interacts with, including the User Service and the Employer Service.

### User Service

The User Service manages user information and authentication. The application interacts with the following endpoints:

- **POST /users**
    - **Description**: Registers a new user with the required details.
    - **Payload**: Includes `email`, `password`, `country`, `access_type`, and optionally `full_name`, `employer_id`, `birth_date`, `salary`.
    - **Conditions for Calling**: Called during the user registration process.

- **GET /users?email=value**
    - **Description**: Retrieves user information based on the email address.
    - **Query Parameter**: `email`
    - **Conditions for Calling**: Used to verify if a user already exists with the given email.

- **GET /users/{id}**
    - **Description**: Fetches details of a user by their unique identifier.
    - **Path Parameter**: `id`
    - **Conditions for Calling**: Used to retrieve user details post-registration or for user profile management.

- **PATCH /users/{id}**
    - **Description**: Updates specific fields of a user's information.
    - **Path Parameter**: `id`
    - **Payload**: An array of objects specifying the `field` to update and its new `value`.
    - **Conditions for Calling**: Called to update user information, such as changing the country.
### UserServiceClient

The `UserServiceClient` serves as an abstraction for interacting with the User Service. It encapsulates the HTTP requests to the User Service endpoints, providing a simplified interface for the application components. Below are the functionalities provided by the `UserServiceClient`:

- **CreateUser**: Invokes the `POST /users` endpoint to register a new user. This is typically called during the user registration process within the application.

- **GetUserByEmail**: Utilizes the `GET /users?email=value` endpoint to retrieve user information based on their email address. This function is essential for checking if a user already exists before attempting to create a new one.

- **GetUserById**: Calls the `GET /users/{id}` endpoint to fetch detailed information about a user by their unique identifier. This is used for user profile management and retrieval operations within the application.

- **UpdateUser**: Makes a request to the `PATCH /users/{id}` endpoint to update specific fields of a user's information. This could be used for various purposes, such as updating a user's country or other profile details.

These functionalities ensure that the application can manage user information effectively without directly handling the HTTP request logic, thereby promoting separation of concerns and simplifying the codebase.
### Employer Service

The Employer Service manages employer-related information. The application interacts with the following endpoint:

- **GET /employers?name=value**
    - **Description**: Searches for employers by name.
    - **Query Parameter**: `name`
    - **Conditions for Calling**: Used to validate employer names during the eligibility file processing or user registration when an employer's name is provided.
### EmployerServiceClient

The `EmployerServiceClient` is designed to facilitate interactions with the Employer Service, abstracting the complexities of HTTP requests and responses. It provides methods for creating employers and retrieving employer information by ID or email. Here's an overview of its capabilities:

- **CreateEmployerAsync**: This method attempts to create a new employer with the provided email. It calls an unspecified endpoint, likely `POST /employers`, and handles HTTP responses. Successful creation results in a `StatusCode` of `Created`, while a `BadRequest` response indicates a failure in creation due to invalid data.

- **GetEmployerByIdAsync**: Fetches employer details based on a unique identifier. This method likely interacts with an endpoint such as `GET /employers/{id}`, where `{id}` is the employer's unique identifier. It returns an `EmployerDto` object if the employer is found (`StatusCode` `OK`), and `null` if not found (`StatusCode` `NotFound`).

- **GetEmployerByEmailAsync**: Similar to `GetEmployerByIdAsync`, this method retrieves employer details based on their email. It likely calls an endpoint such as `GET /employers?email=value`, returning an `EmployerIdRecord` if the employer exists or `null` if there is no employer with the specified email.

These methods ensure that the application can manage employer information efficiently, leveraging the `EmployerServiceClient` for clean and maintainable code.
## Conclusion

This application exemplifies a simple system for processing eligibility files and user registrations, using modern software development practices such as ASP.NET Core, CQRS, and the Mediator pattern with the MediatR library.
#  Sample API Client Library to get data from "Reqres.in"
A .NET library for interacting with the public `https://reqres.in/` API, allowing fetching and processing of user data.

## Project Structure
**SampleCodeForRaftLabs/**: The main class library project.
**Models/**: 
    Contains POCOs/DTOs (`User.cs`, `PagedResult.cs`, `SingleUserResponse.cs`) for API data.
**Services/**:
    `InternalUserService.cs`: For API communication.
    `ExternalUserService.cs`: Service encapsulating data fetching logic (e.g., fetching all users with pagination).
**Exceptions/**: 
    Custom exceptions (`ApiException.cs`, `UserNotFoundException.cs`).

## Features

*   Fetch a single user by ID.
*   Fetch all users (handles API pagination internally).
*   Configurable API base URL.
*   Typed C# models for API responses.
*   Specific exception types for error handling.
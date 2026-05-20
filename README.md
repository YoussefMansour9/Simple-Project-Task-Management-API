# Task Management API

A simple, scalable backend system for Project & Task Management built with ASP.NET Core 9, Clean Architecture, CQRS, and MediatR.

## Features

- **Authentication**: JWT-based authentication with Register and Login
- **Projects Module**: Full CRUD operations for managing projects
- **Tasks Module**: Full CRUD operations for managing tasks with status updates
- **Clean Architecture**: Proper separation of concerns
- **CQRS Pattern**: Using MediatR for command and query separation
- **Redis Caching**: Optional Redis support for caching
- **Docker Support**: Ready for containerized deployment
- **Unit Tests**: Comprehensive unit tests with xUnit

## Tech Stack

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- MediatR (CQRS)
- FluentValidation
- AutoMapper
- xUnit (Testing)
- Docker & Docker Compose

## Architecture

```
TaskManagement.slnx
├── TaskManagement.Domain/          # Domain layer - Entities, Enums, Interfaces
├── TaskManagement.Application/     # Application layer - DTOs, Commands, Queries, Handlers
├── TaskManagement.Infrastructure/  # Infrastructure layer - EF Core, JWT, Redis, Services
├── TaskManagement.Api/            # API layer - Controllers, Middleware, Configuration
└── TaskManagement.Tests/          # Unit tests
```

### Key Principles

- **SOLID Principles**: Each class has a single responsibility
- **Dependency Injection**: All dependencies are injected via constructor
- **Repository Pattern**: Data access is abstracted through repositories
- **Generic Response Wrapper**: Consistent API response format

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server (or Docker)
- Redis (optional)

### Running Locally

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd TaskManagement
   ```

2. **Update connection strings** in `TaskManagement.Api/appsettings.json`
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True",
       "RedisConnection": "localhost:6379"
     }
   }
   ```

3. **Run the application**
   ```bash
   cd TaskManagement.Api
   dotnet run
   ```

4. **Access Swagger UI**: `http://localhost:5000/swagger`

### Running with Docker

1. **Start all services**
   ```bash
   docker-compose up -d
   ```

2. **Access the API**: `http://localhost:8080/swagger`

### Running Tests

```bash
cd TaskManagement.Tests
dotnet test
```

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login and get JWT token |
| GET | `/api/auth/me` | Get current user info |

### Projects

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/projects` | Get all projects |
| GET | `/api/projects/{id}` | Get project by ID |
| POST | `/api/projects` | Create a new project |
| PUT | `/api/projects` | Update a project |
| DELETE | `/api/projects/{id}` | Delete a project |

### Tasks

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tasks/project/{projectId}` | Get tasks by project |
| GET | `/api/tasks/{id}` | Get task by ID |
| POST | `/api/tasks` | Create a new task |
| PUT | `/api/tasks` | Update a task |
| PATCH | `/api/tasks/{id}/status` | Update task status |
| DELETE | `/api/tasks/{id}` | Delete a task |

## Request/Response Examples

### Register User

**Request:**
```http
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Registration successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "email": "john@example.com",
    "fullName": "John Doe",
    "expiresAt": "2024-01-01T12:00:00Z"
  }
}
```

### Create Project

**Request:**
```http
POST /api/projects
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "My Project",
  "description": "Project description"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Project created successfully",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "My Project",
    "description": "Project description",
    "createdAt": "2024-01-01T00:00:00Z",
    "taskCount": 0
  }
}
```

### Create Task

**Request:**
```http
POST /api/tasks
Authorization: Bearer <token>
Content-Type: application/json

{
  "title": "New Task",
  "description": "Task description",
  "priority": "High",
  "projectId": "550e8400-e29b-41d4-a716-446655440000",
  "dueDate": "2024-12-31T23:59:59Z"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Task created successfully",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440001",
    "title": "New Task",
    "description": "Task description",
    "status": "Todo",
    "priority": "High",
    "projectId": "550e8400-e29b-41d4-a716-446655440000",
    "projectName": "My Project",
    "createdAt": "2024-01-01T00:00:00Z"
  }
}
```

## Project Structure

```
TaskManagement/
├── src/
│   ├── TaskManagement.Domain/
│   │   ├── Common/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── Interfaces/
│   ├── TaskManagement.Application/
│   │   ├── Common/
│   │   ├── DTOs/
│   │   ├── Features/
│   │   ├── Interfaces/
│   │   └── Mappings/
│   ├── TaskManagement.Infrastructure/
│   │   ├── Data/
│   │   ├── Repositories/
│   │   └── Services/
│   └── TaskManagement.Api/
│       ├── Controllers/
│       ├── Extensions/
│       └── Middleware/
├── tests/
│   └── TaskManagement.Tests/
├── Dockerfile
├── docker-compose.yml
└── README.md
```

## Configuration

### JWT Settings

```json
{
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!",
    "Issuer": "TaskManagement",
    "Audience": "TaskManagement",
    "ExpirationMinutes": 60
  }
}
```

### Redis (Optional)

For production, Redis connection is configured via `ConnectionStrings:RedisConnection`.

## Error Handling

All errors return a consistent format:

```json
{
  "success": false,
  "message": "Error message",
  "errors": ["Detailed error 1", "Detailed error 2"]
}
```

## Task Status Values

- `Todo`
- `InProgress`
- `Done`

## Task Priority Values

- `Low`
- `Medium`
- `High`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a pull request

## License

This project is licensed under the MIT License.
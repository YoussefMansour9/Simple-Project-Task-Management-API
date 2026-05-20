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
- **Database Migrations**: Entity Framework Core migrations included

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
├── TaskManagement.Application/      # Application layer - DTOs, Commands, Queries, Handlers
├── TaskManagement.Infrastructure/   # Infrastructure layer - EF Core, JWT, Redis, Services
├── TaskManagement.Api/             # API layer - Controllers, Middleware, Configuration
└── TaskManagement.Tests/           # Unit tests
```

### Key Principles

- **SOLID Principles**: Each class has a single responsibility
- **Dependency Injection**: All dependencies are injected via constructor
- **Repository Pattern**: Data access is abstracted through repositories
- **Generic Response Wrapper**: Consistent API response format

## Getting Started

### Prerequisites

- .NET 9 SDK
- Docker & Docker Compose (for containerized setup)
- SQL Server (or use Docker)

### Running with Docker

1. **Start all services**
   ```bash
   docker-compose up -d
   ```

2. **Access the API**: `http://localhost:8080/swagger`
   - API is pre-built and running
   - SQL Server available at localhost:1433
   - Redis available at localhost:6379

3. **Run database migrations** (if needed)
   ```bash
   docker exec -it technicaltest-api-1 dotnet ef database update
   ```
   Note: Database is automatically created on first run via EnsureCreated()

### Running Locally

1. **Clone the repository**
   ```bash
   git clone https://github.com/YoussefMansour9/Simple-Project-Task-Management-API.git
   cd Simple-Project-Task-Management-API
   ```

2. **Start Docker services** (SQL Server & Redis)
   ```bash
   docker-compose up -d db redis
   ```

3. **Run the application**
   ```bash
   cd TaskManagement.Api
   dotnet run
   ```

4. **Access Swagger UI**: `http://localhost:5000/swagger`

### Running Tests

```bash
cd TaskManagement.Tests
dotnet test
```

## Database Migrations

Migrations are located in: `TaskManagement.Api/Data/Migrations/`

To create new migrations:
```bash
cd TaskManagement.Api
dotnet ef migrations add <MigrationName> --output-dir Data/Migrations
```

To apply migrations:
```bash
dotnet ef database update
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

```bash
curl -X POST http://localhost:8080/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"firstName":"John","lastName":"Doe","email":"john@example.com","password":"Password123"}'
```

### Login

```bash
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"john@example.com","password":"Password123"}'
```

### Create Project (requires token)

```bash
curl -X POST http://localhost:8080/api/projects \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"name":"My Project","description":"Description"}'
```

### Create Task

```bash
curl -X POST http://localhost:8080/api/tasks \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"title":"Task Title","description":"Description","priority":"High","projectId":"<project-id>"}'
```

## Configuration

### Connection Strings

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TaskManagementDb;User=sa;Password=YourPassword123;TrustServerCertificate=True",
    "RedisConnection": "localhost:6379"
  }
}
```

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

## Project Structure

```
TaskManagement/
├── TaskManagement.Domain/
│   ├── Common/
│   ├── Entities/
│   ├── Enums/
│   └── Interfaces/
├── TaskManagement.Application/
│   ├── Common/
│   ├── DTOs/
│   ├── Features/
│   ├── Interfaces/
│   └── Mappings/
├── TaskManagement.Infrastructure/
│   ├── Data/
│   ├── Repositories/
│   └── Services/
├── TaskManagement.Api/
│   ├── Controllers/
│   ├── Data/Migrations/
│   ├── Extensions/
│   ├── Middleware/
│   └── Program.cs
├── TaskManagement.Tests/
├── Dockerfile
├── docker-compose.yml
├── TaskManagement.postman_collection.json
└── README.md
```

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

## Important Notes

1. **Database Password**: Default password is `YourPassword123` (change in production)
2. **JWT Secret**: Default secret key should be replaced with a secure value in production
3. **CORS**: Currently configured to allow all origins (configure for production)
4. **Data Persistence**: Docker volumes are used for SQL Server and Redis data persistence

## License

MIT License
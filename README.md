# SimpleMDB - Simple Movie Database API

A RESTful API for managing movies, actors, users, and their relationships built with C# and .NET.

## Project Structure

```
SimpleMovieDatabase/
├── src/
│   └── Smdb/
│       ├── Api/                    # API Layer (Controllers & Routers)
│       │   ├── Movies/
│       │   ├── Actors/
│       │   ├── Users/
│       │   ├── ActorsMovies/
│       │   └── App.cs
│       ├── Core/                   # Business Logic Layer
│       │   ├── Movies/             # Models, Services, Repositories
│       │   ├── Actors/
│       │   ├── Users/
│       │   └── ActorsMovies/
│       ├── Shared/                 # Shared Infrastructure
│       │   ├── Data/               # Database
│       │   └── Http/               # HTTP Framework
│       └── Program.cs
├── appsettings.cfg
├── SimpleMDB_API_Test_cURL.txt
├── SimpleMDB_API_Test_Postman.json
└── SimpleMDB_API_Test_REST_Client.http
```

## Features

- Complete CRUD+L operations for:
  - Movies
  - Actors
  - Users
  - Actor-Movie relationships
- RESTful API design
- Pagination support
- Input validation
- Centralized error handling
- Structured logging
- CORS support

## API Endpoints

All endpoints are under `/api/v1`:

### Movies
- `GET /movies?page=1&size=10` - List movies
- `POST /movies` - Create movie
- `GET /movies/:id` - Get movie by ID
- `PUT /movies/:id` - Update movie
- `DELETE /movies/:id` - Delete movie
- `GET /movies/:id/actors?page=1&size=10` - Get actors in movie

### Actors
- `GET /actors?page=1&size=10` - List actors
- `POST /actors` - Create actor
- `GET /actors/:id` - Get actor by ID
- `PUT /actors/:id` - Update actor
- `DELETE /actors/:id` - Delete actor
- `GET /actors/:id/movies?page=1&size=10` - Get movies by actor

### Users
- `GET /users?page=1&size=10` - List users
- `POST /users` - Create user
- `GET /users/:id` - Get user by ID
- `PUT /users/:id` - Update user
- `DELETE /users/:id` - Delete user

### Actors-Movies
- `GET /actors-movies?page=1&size=10` - List actor-movie relationships
- `POST /actors-movies` - Create relationship
- `GET /actors-movies/:id` - Get relationship by ID
- `PUT /actors-movies/:id` - Update relationship
- `DELETE /actors-movies/:id` - Delete relationship

## Getting Started

### Prerequisites
- .NET SDK 6.0 or higher

### Installation

1. Clone the repository
2. Navigate to the project directory
3. Build the project:
```bash
dotnet build
```

### Running the Server

```bash
cd src/Smdb
dotnet run
```

The server will start at `http://localhost:3000`

## Testing the API

Three test files are provided:

### 1. cURL (SimpleMDB_API_Test_cURL.txt)
Copy commands from the file and run them in your terminal.

Example:
```bash
curl -X GET "http://localhost:3000/api/v1/movies?page=1&size=5"
```

### 2. Postman (SimpleMDB_API_Test_Postman.json)
1. Open Postman
2. Click "Import"
3. Select `SimpleMDB_API_Test_Postman.json`
4. Run requests from the collection

### 3. REST Client (SimpleMDB_API_Test_REST_Client.http)
1. Install the "REST Client" extension in VS Code
2. Open `SimpleMDB_API_Test_REST_Client.http`
3. Click "Send Request" above any request

## Sample Data

The application comes pre-seeded with:
- 2 Users (admin, user1)
- 3 Movies (The Shawshank Redemption, The Godfather, The Dark Knight)
- 3 Actors (Morgan Freeman, Marlon Brando, Christian Bale)
- 3 Actor-Movie relationships

## Architecture

The application follows a layered architecture:

1. **API Layer** - Controllers and Routers handle HTTP requests/responses
2. **Service Layer** - Business logic and validation
3. **Repository Layer** - Data access abstraction
4. **Model Layer** - Domain entities

This separation ensures:
- Clean code organization
- Easy testing
- Maintainability
- Scalability

## Error Handling

The API returns appropriate HTTP status codes:
- `200 OK` - Successful GET/PUT/DELETE
- `201 Created` - Successful POST
- `400 Bad Request` - Validation errors
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server errors

Error responses include a message:
```json
{
  "error": "Error message here"
}
```

## Configuration

Edit `appsettings.cfg` to change server settings:
```
HOST=http://localhost
PORT=3000
```

## License

This is an educational project for learning purposes.

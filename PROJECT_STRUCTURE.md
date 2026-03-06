# SimpleMDB Project Structure Guide

This document explains the architecture and file organization for your video walkthrough.

## High-Level Architecture

```
Client Request
    ↓
HTTP Server (App.cs)
    ↓
Middleware Pipeline (Logging, Error Handling, CORS, etc.)
    ↓
Router (Maps URL to Controller)
    ↓
Controller (Handles HTTP Request/Response)
    ↓
Service (Business Logic & Validation)
    ↓
Repository (Data Access)
    ↓
Database (In-Memory)
```

## Directory Structure Explained

### `/src/Smdb/Shared/` - Infrastructure Layer

#### `Shared/Http/`
- **HttpServer.cs** - Base HTTP server that listens for requests
- **HttpRouter.cs** - Routes requests to appropriate handlers
- **HttpUtils.cs** - Middleware functions (logging, error handling, CORS, etc.)
- **Result.cs** - Wrapper for operation results (success/error)
- **PagedResult.cs** - Wrapper for paginated data

#### `Shared/Data/`
- **MemoryDatabase.cs** - In-memory database with seed data

### `/src/Smdb/Core/` - Business Logic Layer

Each domain (Movies, Actors, Users, ActorsMovies) has:

#### Models
- **Movie.cs** / **Actor.cs** / **User.cs** / **ActorMovie.cs**
- Plain C# classes representing domain entities
- No logic, just properties

#### Repository Interfaces
- **IMovieRepository.cs** / **IActorRepository.cs** / etc.
- Defines contract for data access
- Allows swapping implementations (memory → SQL → NoSQL)

#### Repository Implementations
- **MemoryMovieRepository.cs** / **MemoryActorRepository.cs** / etc.
- Implements data access using MemoryDatabase
- Converts between Dictionary and Model objects

#### Service Interfaces
- **IMovieService.cs** / **IActorService.cs** / etc.
- Defines contract for business operations
- Returns Result<T> objects

#### Service Implementations
- **DefaultMovieService.cs** / **DefaultActorService.cs** / etc.
- Implements business logic
- Validates input
- Calls repository
- Wraps results with appropriate HTTP status codes

### `/src/Smdb/Api/` - API Layer

Each domain has:

#### Controllers
- **MoviesController.cs** / **ActorsController.cs** / etc.
- Handles HTTP requests
- Extracts parameters from URL and query string
- Deserializes JSON from request body
- Calls service methods
- Serializes responses as JSON

#### Routers
- **MoviesRouter.cs** / **ActorsRouter.cs** / etc.
- Maps HTTP methods and URL patterns to controller methods
- Configures middleware per route

#### App
- **App.cs** - Main application entry point
  - Initializes all components
  - Wires up dependency injection
  - Configures global middleware
  - Sets up routing hierarchy

### Root Files

- **Program.cs** - Application entry point, creates and starts App
- **appsettings.cfg** - Configuration (host, port)

## Request Flow Example: POST /api/v1/movies

### 1. Client sends request
```bash
curl -X POST "http://localhost:3000/api/v1/movies" \
  -H "Content-Type: application/json" \
  -d '{"title":"Inception","year":2010,"description":"..."}'
```

### 2. HttpServer (App.cs) receives request
- `App.Start()` listens on port 3000
- Receives HttpListenerContext
- Creates props Hashtable for request data

### 3. Global Middleware Pipeline
Executes in order:
1. **StructuredLogging** - Logs request method and path
2. **CentralizedErrorHandling** - Wraps execution in try-catch
3. **AddResponseCorsHeaders** - Adds CORS headers
4. **DefaultResponse** - Sets 404 if no route matches
5. **ParseRequestUrl** - Extracts path from URL
6. **ParseRequestQueryString** - Parses query parameters

### 4. Router Matching
- Main router checks path starts with `/api/v1`
- Delegates to API router
- API router checks path starts with `/movies`
- Delegates to Movies router
- Movies router matches `POST /` pattern

### 5. Route-Specific Middleware
- **ReadRequestBodyAsText** - Reads JSON from request body into props["req.text"]

### 6. MoviesController.CreateMovie()
```csharp
public async Task CreateMovie(...)
{
    // 1. Get JSON from props
    var json = props["req.text"]?.ToString() ?? "";
    
    // 2. Deserialize to Movie object
    var movie = JsonSerializer.Deserialize<Movie>(json);
    
    // 3. Call service
    var result = await movieService.CreateMovie(movie);
    
    // 4. Send response
    await HttpUtils.SendResultResponse(req, res, props, result);
}
```

### 7. DefaultMovieService.CreateMovie()
```csharp
public async Task<Result<Movie>> CreateMovie(Movie newMovie)
{
    // 1. Validate input
    var validationResult = ValidateMovie(newMovie);
    if (validationResult != null)
        return validationResult; // Returns 400 Bad Request
    
    // 2. Call repository
    var movie = await movieRepository.CreateMovie(newMovie);
    
    // 3. Wrap result
    return new Result<Movie>(movie, 201); // 201 Created
}
```

### 8. MemoryMovieRepository.CreateMovie()
```csharp
public Task<Movie?> CreateMovie(Movie movie)
{
    // 1. Generate new ID
    movie.Id = db.GetNextMovieId();
    
    // 2. Convert to dictionary
    var dict = MovieToDict(movie);
    
    // 3. Add to database
    db.Movies.Add(dict);
    
    // 4. Return movie
    return Task.FromResult<Movie?>(movie);
}
```

### 9. Response flows back
- Repository returns Movie to Service
- Service wraps in Result<Movie> with status 201
- Controller serializes Result to JSON
- HttpUtils.SendResultResponse writes to response stream
- Middleware completes
- Response sent to client

### 10. Client receives response
```json
{
  "Id": 4,
  "Title": "Inception",
  "Year": 2010,
  "Description": "..."
}
```
Status: 201 Created

## Error Flow Example: POST with Invalid Data

### Request
```json
{
  "title": "",
  "year": 2010,
  "description": "Test"
}
```

### Flow
1. Same path through Server → Middleware → Router → Controller
2. Controller deserializes JSON successfully
3. Service validates input
4. **ValidateMovie()** detects empty title
5. Returns `Result<Movie>` with error and status 400
6. Controller serializes error response
7. Client receives:
```json
{
  "error": "Title is required and cannot be empty."
}
```
Status: 400 Bad Request

## Key Design Patterns

### 1. Layered Architecture
- Separation of concerns
- Each layer has specific responsibility
- Changes in one layer don't affect others

### 2. Dependency Injection
- Components receive dependencies through constructors
- Enables testing and flexibility
- Example: Service receives Repository interface

### 3. Repository Pattern
- Abstracts data access
- Can swap implementations without changing business logic
- Interface defines contract

### 4. Result Pattern
- Wraps operation results
- Includes success data OR error
- Includes HTTP status code
- Avoids throwing exceptions for expected errors

### 5. Middleware Pipeline
- Request passes through chain of functions
- Each middleware can modify request/response
- Can short-circuit pipeline (e.g., error handling)

## Testing Strategy

### Unit Tests (Not included, but recommended)
- Test services with mock repositories
- Test validation logic
- Test error handling

### Integration Tests (Not included, but recommended)
- Test full request/response cycle
- Verify status codes
- Verify response payloads

### Manual Tests (Provided)
- cURL commands
- Postman collection
- REST Client file

## For Your Video Walkthrough

### Successful Request (200/201)
Pick: `POST /api/v1/movies` (Create)
1. Show the cURL command
2. Open App.cs - show Init() wiring
3. Show middleware pipeline execution
4. Show routing to MoviesRouter
5. Show MoviesController.CreateMovie()
6. Show DefaultMovieService.CreateMovie() validation
7. Show MemoryMovieRepository.CreateMovie() data access
8. Show response flowing back
9. Show JSON response with 201 status

### Error Request (400)
Pick: `POST /api/v1/movies` with empty title
1. Show the cURL command with invalid data
2. Follow same path to Service
3. Highlight ValidateMovie() catching the error
4. Show Result<Movie> with error and 400 status
5. Show error response JSON

### Error Request (404)
Pick: `GET /api/v1/movies/999`
1. Show the cURL command
2. Follow to Repository
3. Show repository returning null
4. Show Service wrapping null as error with 404
5. Show error response JSON

This structure demonstrates professional software engineering principles while remaining simple enough to understand and explain.

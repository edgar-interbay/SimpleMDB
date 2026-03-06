# Quick Start Guide - SimpleMDB API

## Running the Server

1. Open a terminal in the project root directory
2. Navigate to the source folder:
   ```bash
   cd src/Smdb
   ```
3. Run the application:
   ```bash
   dotnet run
   ```
4. You should see:
   ```
   Server running at http://localhost:3000/
   Press Ctrl+C to stop
   ```

## Testing with cURL

Open a new terminal (keep the server running) and try these commands:

### List all movies
```bash
curl -X GET "http://localhost:3000/api/v1/movies?page=1&size=5"
```

### Get a specific movie
```bash
curl -X GET "http://localhost:3000/api/v1/movies/1"
```

### Create a new movie
```bash
curl -X POST "http://localhost:3000/api/v1/movies" \
  -H "Content-Type: application/json" \
  -d '{"title":"Inception","year":2010,"description":"A mind-bending thriller"}'
```

### Update a movie
```bash
curl -X PUT "http://localhost:3000/api/v1/movies/1" \
  -H "Content-Type: application/json" \
  -d '{"title":"The Shawshank Redemption - Updated","year":1994,"description":"Updated description"}'
```

### Delete a movie
```bash
curl -X DELETE "http://localhost:3000/api/v1/movies/3"
```

## Testing with Postman

1. Open Postman
2. Click "Import" button
3. Select the file `SimpleMDB_API_Test_Postman.json`
4. You'll see a collection with all test requests organized by resource
5. Click any request and hit "Send"

## Testing with REST Client (VS Code)

1. Install the "REST Client" extension in VS Code
2. Open the file `SimpleMDB_API_Test_REST_Client.http`
3. You'll see requests separated by `###`
4. Click "Send Request" that appears above each request
5. Results will appear in a new panel

## Understanding the Responses

### Success Response (200/201)
```json
{
  "Id": 1,
  "Title": "The Shawshank Redemption",
  "Year": 1994,
  "Description": "Two imprisoned men bond over a number of years."
}
```

### Paginated Response
```json
{
  "Items": [...],
  "Page": 1,
  "Size": 5,
  "TotalItems": 3,
  "TotalPages": 1
}
```

### Error Response (400/404)
```json
{
  "error": "Title is required and cannot be empty."
}
```

## Common Test Scenarios

### Successful Operations
- List resources with valid pagination
- Create resources with valid data
- Read existing resources
- Update existing resources with valid data
- Delete existing resources

### Error Operations
- List with invalid page/size (page=0, size=-1)
- Create with missing required fields
- Create with invalid data (year=1500)
- Read non-existent resources (id=999)
- Update non-existent resources
- Delete non-existent resources

## All Available Endpoints

### Movies
- `GET /api/v1/movies?page=1&size=10`
- `POST /api/v1/movies`
- `GET /api/v1/movies/:id`
- `PUT /api/v1/movies/:id`
- `DELETE /api/v1/movies/:id`
- `GET /api/v1/movies/:id/actors?page=1&size=10`

### Actors
- `GET /api/v1/actors?page=1&size=10`
- `POST /api/v1/actors`
- `GET /api/v1/actors/:id`
- `PUT /api/v1/actors/:id`
- `DELETE /api/v1/actors/:id`
- `GET /api/v1/actors/:id/movies?page=1&size=10`

### Users
- `GET /api/v1/users?page=1&size=10`
- `POST /api/v1/users`
- `GET /api/v1/users/:id`
- `PUT /api/v1/users/:id`
- `DELETE /api/v1/users/:id`

### Actors-Movies
- `GET /api/v1/actors-movies?page=1&size=10`
- `POST /api/v1/actors-movies`
- `GET /api/v1/actors-movies/:id`
- `PUT /api/v1/actors-movies/:id`
- `DELETE /api/v1/actors-movies/:id`

## Tips for Video Demonstrations

### For the 5-minute CRUD+L demo:
1. Start the server
2. Show 5 successful operations (List, Create, Read, Update, Delete)
3. Show 5 error operations (invalid inputs, not found, etc.)
4. Do this for one resource using all three tools (cURL, Postman, REST Client)

### For the 10-minute code walkthrough:
1. Start with a client request (e.g., POST /api/v1/movies)
2. Show how it enters App.cs
3. Follow through middleware (logging, error handling, CORS)
4. Show routing to MoviesRouter
5. Show MoviesController receiving the request
6. Show MovieService validating and processing
7. Show MovieRepository accessing data
8. Show response flowing back through the layers
9. Do this for both a successful request (201) and an error request (400)

## Stopping the Server

Press `Ctrl+C` in the terminal where the server is running.

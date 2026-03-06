# SimpleMDB Project Deliverables Checklist

## Project Requirements Summary

This checklist helps you verify all requirements are met before submission.

---

## ✅ 1. GitHub Repository [40 Points]

### Required API Endpoints (21 total)

#### Users [10 Points]
- [ ] `GET /api/v1/users?page=1&size=10` - List users
- [ ] `POST /api/v1/users` - Create user
- [ ] `GET /api/v1/users/:id` - Read user
- [ ] `PUT /api/v1/users/:id` - Update user
- [ ] `DELETE /api/v1/users/:id` - Delete user

#### Movies [10 Points]
- [ ] `GET /api/v1/movies?page=1&size=10` - List movies
- [ ] `POST /api/v1/movies` - Create movie
- [ ] `GET /api/v1/movies/:id` - Read movie
- [ ] `PUT /api/v1/movies/:id` - Update movie
- [ ] `DELETE /api/v1/movies/:id` - Delete movie
- [ ] `GET /api/v1/movies/:id/actors?page=1&size=10` - Get actors in movie

#### Actors [10 Points]
- [ ] `GET /api/v1/actors?page=1&size=10` - List actors
- [ ] `POST /api/v1/actors` - Create actor
- [ ] `GET /api/v1/actors/:id` - Read actor
- [ ] `PUT /api/v1/actors/:id` - Update actor
- [ ] `DELETE /api/v1/actors/:id` - Delete actor
- [ ] `GET /api/v1/actors/:id/movies?page=1&size=10` - Get movies by actor

#### Actors-Movies [10 Points]
- [ ] `GET /api/v1/actors-movies?page=1&size=10` - List relationships
- [ ] `POST /api/v1/actors-movies` - Create relationship
- [ ] `GET /api/v1/actors-movies/:id` - Read relationship
- [ ] `PUT /api/v1/actors-movies/:id` - Update relationship
- [ ] `DELETE /api/v1/actors-movies/:id` - Delete relationship

### Repository Checklist
- [ ] Project compiles without errors (`dotnet build`)
- [ ] All source code is authored by you (not copied)
- [ ] Repository is accessible via GitHub link
- [ ] README.md explains how to run the project
- [ ] .gitignore excludes build artifacts

---

## ✅ 2. Video Demonstration (5 minutes) [30 Points]

### cURL Tests [10 Points]
Record yourself running these commands:

#### 5 Successful Operations
- [ ] List movies: `curl -X GET "http://localhost:3000/api/v1/movies?page=1&size=5"`
- [ ] Create movie: `curl -X POST ... (with valid data)`
- [ ] Read movie: `curl -X GET "http://localhost:3000/api/v1/movies/1"`
- [ ] Update movie: `curl -X PUT ... (with valid data)`
- [ ] Delete movie: `curl -X DELETE "http://localhost:3000/api/v1/movies/3"`

#### 5 Unsuccessful Operations
- [ ] List with invalid page: `curl -X GET "...?page=0&size=5"` → 400
- [ ] Create with empty title: `curl -X POST ... (empty title)` → 400
- [ ] Read non-existent: `curl -X GET ".../999"` → 404
- [ ] Update with invalid year: `curl -X PUT ... (year=1500)` → 400
- [ ] Delete non-existent: `curl -X DELETE ".../999"` → 404

### Postman Tests [10 Points]
Record yourself using Postman:

#### 5 Successful Operations
- [ ] List movies (200)
- [ ] Create movie (201)
- [ ] Read movie (200)
- [ ] Update movie (200)
- [ ] Delete movie (200)

#### 5 Unsuccessful Operations
- [ ] Invalid pagination (400)
- [ ] Invalid input data (400)
- [ ] Resource not found (404)
- [ ] Validation error (400)
- [ ] Another not found (404)

### REST Client Tests [10 Points]
Record yourself using VS Code REST Client:

#### 5 Successful Operations
- [ ] List movies
- [ ] Create movie
- [ ] Read movie
- [ ] Update movie
- [ ] Delete movie

#### 5 Unsuccessful Operations
- [ ] Invalid page/size
- [ ] Missing required field
- [ ] Not found error
- [ ] Validation error
- [ ] Another error case

---

## ✅ 3. Code Walkthrough Video (10 minutes) [30 Points]

### Successful Request Walkthrough [15 Points]
Pick: `POST /api/v1/movies` (201 Created)

Show and explain:
- [ ] Client request (cURL/Postman command)
- [ ] App.cs - Server initialization and middleware setup
- [ ] Middleware pipeline execution (logging, error handling, CORS)
- [ ] Router matching (main → api → movies)
- [ ] MoviesRouter - Route configuration
- [ ] MoviesController.CreateMovie() - Request handling
- [ ] DefaultMovieService.CreateMovie() - Business logic & validation
- [ ] MemoryMovieRepository.CreateMovie() - Data access
- [ ] Response flowing back through layers
- [ ] JSON response with 201 status code

### Unsuccessful Request Walkthrough [15 Points]
Pick: `POST /api/v1/movies` with invalid data (400 Bad Request)

Show and explain:
- [ ] Client request with invalid data
- [ ] Same path through App → Middleware → Router → Controller
- [ ] Service layer validation catching the error
- [ ] ValidateMovie() method detecting the problem
- [ ] Result<Movie> with error and 400 status
- [ ] Error response flowing back
- [ ] JSON error response with 400 status code

### BONUS: Server Error Walkthrough
Pick: Deliberately throw exception (500 Internal Server Error)

Show and explain:
- [ ] Request that triggers an exception
- [ ] CentralizedErrorHandling middleware catching it
- [ ] Logging the error
- [ ] Returning 500 status with generic error message

---

## Video Recording Tips

### For 5-Minute Demo Video
1. **Preparation**
   - Start server before recording
   - Have all commands ready in a text file
   - Test everything works before recording
   - Keep terminal font size large and readable

2. **Recording Structure**
   - Introduction (15 sec): "Testing SimpleMDB API with [tool]"
   - Successful operations (2 min): Show 5 working requests
   - Error operations (2 min): Show 5 error cases
   - Repeat for each tool (cURL, Postman, REST Client)

3. **What to Show**
   - The command/request being sent
   - The response received
   - The HTTP status code
   - Brief explanation of what's happening

### For 10-Minute Code Walkthrough Video
1. **Preparation**
   - Open all relevant files in VS Code
   - Use split view to show multiple files
   - Increase font size for readability
   - Have a clear path through the code

2. **Recording Structure**
   - Introduction (30 sec): Overview of architecture
   - Successful request (4 min): Follow request through all layers
   - Error request (4 min): Follow error through all layers
   - Conclusion (1.5 min): Summary of architecture benefits

3. **What to Explain**
   - Purpose of each layer
   - How data flows between layers
   - Why separation of concerns matters
   - How error handling works
   - Benefits of this architecture

---

## Pre-Submission Checklist

### Code Quality
- [ ] Project compiles: `dotnet build`
- [ ] Server runs: `dotnet run`
- [ ] All endpoints respond correctly
- [ ] Error handling works properly
- [ ] Code is well-organized and readable

### Documentation
- [ ] README.md is complete
- [ ] QUICK_START.md helps with testing
- [ ] Test files are included (cURL, Postman, REST Client)
- [ ] Comments explain complex logic

### GitHub Repository
- [ ] Repository is public or accessible
- [ ] All files are committed
- [ ] .gitignore excludes build artifacts
- [ ] Repository URL is ready to submit

### Videos
- [ ] 5-minute demo video is recorded
- [ ] 10-minute walkthrough video is recorded
- [ ] Videos are clear and audible
- [ ] Videos demonstrate all requirements
- [ ] Videos are uploaded and accessible

---

## Submission Format

Submit:
1. **GitHub Repository Link**
   - Example: `https://github.com/yourusername/SimpleMDB`

2. **5-Minute Demo Video Link**
   - Upload to YouTube, Google Drive, or similar
   - Ensure link is accessible

3. **10-Minute Walkthrough Video Link**
   - Upload to YouTube, Google Drive, or similar
   - Ensure link is accessible

---

## Grading Breakdown

| Component | Points | Status |
|-----------|--------|--------|
| GitHub Repository - Users | 10 | ⬜ |
| GitHub Repository - Movies | 10 | ⬜ |
| GitHub Repository - Actors | 10 | ⬜ |
| GitHub Repository - Actors-Movies | 10 | ⬜ |
| Demo Video - cURL | 10 | ⬜ |
| Demo Video - Postman | 10 | ⬜ |
| Demo Video - REST Client | 10 | ⬜ |
| Walkthrough - Successful Request | 15 | ⬜ |
| Walkthrough - Error Request | 15 | ⬜ |
| **TOTAL** | **100** | |
| **BONUS - Server Error** | +5 | ⬜ |

---

## Common Mistakes to Avoid

1. ❌ Repository doesn't compile
2. ❌ Missing endpoints
3. ❌ Video doesn't show both successful AND error cases
4. ❌ Code walkthrough doesn't follow request through all layers
5. ❌ Video links are not accessible
6. ❌ Code is copied from another student
7. ❌ Server not running during demo
8. ❌ Font too small to read in video
9. ❌ No audio explanation in walkthrough video
10. ❌ Incomplete test files

---

## Need Help?

- Review `README.md` for setup instructions
- Review `QUICK_START.md` for testing guide
- Review `PROJECT_STRUCTURE.md` for architecture explanation
- Test all endpoints before recording
- Practice your walkthrough before recording

Good luck! 🚀

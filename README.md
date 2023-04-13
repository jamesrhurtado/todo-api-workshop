# C# TodoList API Project
This is a C# project that creates a TodoList API using .NET and SQLite for the database.

### Technologies Used
- .NET
- C#
- SQLite


### Setup

1. Install .NET if not already installed.
2. Clone the repository.
3. Open the project in Visual Studio.
4. Run the following command to restore the project dependencies:
```
dotnet restore
```
5. Build the project.
6. Run the following command to start the application:
```
dotnet run
```
Make sure to run these commands from the root directory of the project. This will download and install any necessary dependencies and run the project respectively.


### API Endpoints
#### GET /api/todo
Returns a list of all Todo items.

#### GET /api/todo/{id}
Returns the Todo item with the specified ID.

#### POST /api/todo
Creates a new Todo item.

#### PUT /api/todo/{id}
Updates the Todo item with the specified ID.

#### DELETE /api/todo/{id}
Deletes the Todo item with the specified ID.

### Database
The project uses SQLite for the database.

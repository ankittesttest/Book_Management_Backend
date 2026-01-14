# Bookshelf Management System

A full-stack application for managing bookshelves and books with drag-and-drop functionality.

## Prerequisites

- .NET 10 SDK
- SQL Server
- Node.js (v24 or higher)
- Angular CLI ("npm install -g @angular/cli@21")

## Backend Setup (.NET 10 Web API)

### 1. Create the project structure
Commands
mkdir BookManagement
cd BookManagement
dotnet new webapi -n BookManagement
cd BookManagement


### 2. Install required packages
Commands
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Swashbuckle.AspNetCore


### 3. Create folder structure


BookshelfAPI
|
|-- Models
|   |-- Bookshelf.cs
|   |-- Book.cs
|
|-- Data
|   |-- BookshelfContext.cs
|
|-- Controllers
|   |-- BookshelvesController.cs
|   |-- BooksController.cs
|
|-- Program.cs
|-- appsettings.json
|-- BookshelfAPI.csproj


### 4. Copy the provided files into their respective locations

- Copy all the C# code files to their folders
- Update "appsettings.json" with your SQL Server connection string
- Default connection string: "Server=localhost;Database=BookshelfDB;Trusted_Connection=True;TrustServerCertificate=True;"

### 5. Create and seed the database

# You need to use migrations:
Commands
dotnet ef migrations add InitialCreate
dotnet ef database update

### 6. Run the API
Command
dotnet run


The API will be available at "https://localhost:7213" (or the port shown in your console).
Swagger UI will be available at "https://localhost:7213/swagger"

## Frontend Setup (Angular 21)

### 1. Create Angular project
Commands
ng new crud-angular --routing --style=css --standalone
cd crud-angular

### 2. Install Angular CDK for drag-and-drop
Command
npm install @angular/cdk@19


### 3. Create folder structure
Commands
"ng g c bookshelf"
"ng g c notfound"
"ng g i models/models"
"ng g s services/bookshelf"

src/app
|
|-- models
|   |-- models.ts
|
|-- services
|   |-- bookshelf.service.ts
|
|-- components
|   |--notfound
|   |  |-- notfound.component.ts
|   |  |-- notfound.component.html
|   |  |-- notfound.component.css
|   |
|   |-- bookshelf
|       |-- bookshelf.component.ts
|       |-- bookshelf.component.html
|       |-- bookshelf.component.css
|
|-- app.component.ts
|-- app.config.ts
|-- app.routes.ts

### 4. Update the API URL in the provided files

- Update the API URL in "bookshelf.service.ts" if your API runs on a different port

### 5. Update API URL (if needed)

In "src/app/services/bookshelf.service.ts", update the "apiUrl" to match your backend:

typescript "private apiUrl = 'https://localhost:7213/api';"

### 6. Run the Angular app
Command 
"ng serve"

The app will be available at "http://localhost:4200"

## Features

### Backend API Endpoints

## Bookshelves:
- "GET /api/Bookshelves" - Get all bookshelves with books
- "GET /api/Bookshelves/{id}" - Get a specific bookshelf
- "POST /api/Bookshelves" - Create a new bookshelf
- "PUT /api/Bookshelves/{id}" - Update a bookshelf
- "DELETE /api/Bookshelves/{id}" - Delete a bookshelf

## Books:
- "GET /api/Books" - Get all books
- "GET /api/Books/{id}" - Get a specific book
- "POST /api/Books" - Add a new book
- "PUT /api/Books/{id}" - Update a book
- "DELETE /api/Books/{id}" - Delete a book
- "POST /api/Books/MoveBook" - Move a book to a different position/bookshelf

### Frontend Features

- View all bookshelves and their books
- Add new bookshelves with name, description, and capacity
- Add new books to any bookshelf
- Drag and drop books to reorder them within a bookshelf
- Drag and drop books between different bookshelves
- Delete books and bookshelves
- Responsive design

## Database Schema
- We have two table to manage this. 

**Bookshelves Table:**
- Id (Primery Key)
- Name
- Description
- Capacity

**Books Table:**
- Id (Primery Key)
- Title
- Author
- Position
- BookshelfId (Foreign Key)

## Troubleshooting

### API Connection Issues

1. Make sure the API is running and accessible
2. Check CORS settings in "Program.cs"
3. Verify the API URL in "bookshelf.service.ts" matches your API address
4. Check browser console for CORS or network errors
5. For the api verfication you can also use the swagger.

### Database Connection Issues

1. Ensure SQL Server is installed and running
2. Update the connection string in "appsettings.json" with your server details
3. Try using SQL Server Authentication if Windows Authentication fails. Actually I have also used this :)
   "Server=localhost;Database=BookshelfDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"

### Angular Build Issues

1. Make sure you are using Node.js v24 or higher
2. Clear node_modules and reinstall: "rm -rf node_modules && npm install"
3. Check that @angular/cdk is installed: "npm list @angular/cdk"

## Testing the Application

1. Start the .NET API first
2. Start the Angular app
3. Open "http://localhost:4200" in your browser
4. You should add bookshelves and books using button.
5. Try dragging books between shelves
6. Add new books and bookshelves
7. Delete items to test the full CRUD functionality

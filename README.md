# Task Manager API

## Description

This is a simple RESTful API built with ASP.NET Core to manage a to-do list application. The application allows users to:

- Create a new task note
- Retrieve a list of all task notes
- Retrieve a task note by ID
- Update a task note
- Delete a task note

It uses MySQL as the database for persisting task notes, and Docker Compose is used to set up the application and MySQL database together.

## Technologies Used

- ASP.NET Core
- Entity Framework Core (EF Core)
- MySQL
- Docker
- xUnit (for unit tests)

## Requirements

To run this application, you need Docker and Docker Compose installed on your machine.

## Running the Application

1. Clone the repository to your local machine.
2. Open a terminal and navigate to the root directory of the project.
3. Run the following command:

   ```bash
   docker-compose up

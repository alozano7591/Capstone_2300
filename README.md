# Introduction 
This is a capstone project for an app that provides EV owners with access to a wider network of charging options.

# Project and App Name
- Circuit Share

# Team 
- Alfredo Lozano : alozano7591@conestogac.on.ca
- Austin Cameron : acameron1391@conestogac.on.ca
- Cody Botelho : cbotelho7151@conestogac.on.ca
- Elliot Stronge : estronge0011@conestogac.on.ca

# Software Requirements
To run the projects users will require the following:
- Software that can run ASP.net core MVC (Visual Studio, IntelliJ, etc)
- PostgreSQL

# Primary Build
First, you need to build the database. In order to do this:

1.  Open the project in Visual studio
2.  Go to server explorer and connect to server
3.  Open Nuget Package Manager Console
4.  Run the following commands:
    - run command in db console `add-migration migrationName`
    - run command in db console `update-database`

# To fully clear database 
- run command `drop-database`.
- delete mirgrations folder (we do not have any important data for now, so delete whenever needed).

# Troubleshooting
-   There is a chance that the connection string for PostgreSQL may not be compatible with your setup.
    If so, go to the file "appsettings.json" and update the connection string username and password 
    with your local PostgreSQL details.

# Build and Test
Building should be pretty simple in Visual Studio. Simply press the "run" button and the project should 
build and run.  


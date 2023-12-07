# Introduction 
MVC Web App with the purpose of storing passwords for the user.

# TODO
1. Add Homepage styling
2. Fix redundant code
3. Unit tests

# Getting Started
To start using the application just install **Docker** and open the terminal in the project directory, then type **docker-compose up**. 
The command will pull the images of both the database and the application through the **Dockerfile**. 
Wait for the installation to end and close the container. When restarting it, the app will be available to use in the browser locally on the 8000 port.

# Build and Test
If the user wishes to test the application or rebuild it, just install any IDE and edit the source code (any version of Visual Studio is reccomended). 
If the user wants to make changes to the data inside the db, use SQL Server Management Studio and fill in the fields for the connection present in the docker-compose file. Use migrations to change the schema of the db.


# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 
If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)
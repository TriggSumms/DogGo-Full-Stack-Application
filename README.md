# DogGo-Full-Stack-Application
# Introduction to ASP.NET MVC Web Application

In this chapter you'll create a new MVC project to start the Nashville dog walking application, DogGo.

## Getting Started

1. Create new project in Visual Studio
1. Choose the _ASP.NET Core Web Application_
1. Specify project name of _DogGo_
1. Click _Ok_
1. Choose _Web Application (Model-View-Controller)_
1. Click _Ok_
1. Add the Nuget package for `Microsoft.Data.SqlClient`

Take a look around at the project files that come out of the box with a new ASP.NET MVC project. It already has folders for Models, Views, and Controllers. It has a `wwwroot` folder which contains some static assets like javascript and css files. It has a `Startup.cs` file where we can configure certain things about our web application if we choose.

## Database

Run the [dog walker sql script](./assets/DogWalker.sql) to create database. Take a moment and look through the tables that get created.

## Configuration

Open the `appsettings.json` file and add your connection string. The file should look like this

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DogWalkerMVC;Trusted_Connection=True;"
  }
}
```

## Exercise

1. Create an `OwnerRepository` and an `OwnersController` file and implement the `Index` and `Details` methods.
1. Go into the `Shared` folder in the `_Layout.cshtml` file. Add links for "Walkers" and "Owners" in the navbar. If you finish, try changing the views and the styling to your liking.
1. **Challenge**: When viewing the details page of an owner, list all the dogs for that owner as well.

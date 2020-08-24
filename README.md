# Example project IMD

## Introduction

This project contains a very basic example API and can be used as a starting point for other projects.

## Installation

You will need the following software:

- [.NET Core (At least version 3.1)](https://dotnet.microsoft.com/download).
- An editor/IDE of your choice. Personally I use [Visual Studio Code](https://code.visualstudio.com/). When you open a `.cs` file from the project it will suggest an extension (C#); installing that add intellisense. See the last point in this document for a list of suggested extensions.

## Usage

You can use `dotnet watch run` to run the project; after you get the notification that the application started navigate to <https://localhost:5001/swagger/index.html>; you will get an overview with all the API methods and a quick method to execute them.

Read through the code; it has been extensively commented.

### Explanation

- [dotnet watch run](https://docs.microsoft.com/en-us/aspnet/core/tutorials/dotnet-watch?view=aspnetcore-3.1) is a file monitor; it compiles and watches your code. You could also use `dotnet run` - however, you will need to restart that process on every change.
- You should know the basic principles of [object-oriented programming](https://en.wikipedia.org/wiki/Object-oriented_programming) and know what [an API is](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design).

## Exercises for the reader

- Add extra endpoints to manipulate the cars in the database.
- [Refactor](https://wiki.c2.com/?WhatIsRefactoring) the code.

## Suggested visual studio extensions

- [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
- [Docker](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-docker)

### Suggested editor settings

To change this, open your settings via `ctrl+p` and type settings. One of the options should be "Preferences: Open Settings", this opens a json file where you might (or might not) have the settings of your editor.

```json
{
  "editor.formatOnSave": true
}
```

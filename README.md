SQL Query Generator Tool
======
A simple .NET 5 cli tool to generate insert/update sql queries easily.

## About
As a full stack developer I sometimes had to create multiple insert/update queries for my database entities which can be a very tedious and boring task. Some database management tools allow you to create them with a few clicks but only one table at a time (at least the ones I've tried) and you need to spend time fixing the indentation those tools provide if you don't like them. With that in mind, I decided to create this simple tool to help me create queries for multiple tables at once with full control of the output.

## Usage
In order to use this tool you need to provide the **database name** and your preferred **authentication method**.

Example: `sqgt -db dbName --integrated`

This will attempt to connect to the `dbName` database with integrated security authentication.

#### Authentication
You can either authenticate to a database either by using integrated authentication or databse credentials, but you need to specify it since there's no default method.

**Integrated Authentication**
`sqgt -db dbName --integrated`

**Credentials Authentication**
`sqgt -db dbName -u dbUsername -pw dbPassword`

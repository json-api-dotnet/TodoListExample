# JsonApiDotNetCore Todo List Example

Demo application for [jsonapi-dotnet-core](https://github.com/json-api-dotnet/JsonApiDotNetCore/).

[![Goto Playlist](https://img.youtube.com/vi/KAMuo6K7VcE/0.jpg)](https://www.youtube.com/watch?v=KAMuo6K7VcE&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf)

## Usage

### Starting the API

(requires running postgres instance with credentials specified in appsettings.json)

- `cd TodoListAPI`
- Run migrations: `dotnet ef database update`
- Set the environment to development: `export ASPNETCORE_ENVIRONMENT=development` (mac)
- `dotnet run`

### Starting the Client

- Install packges: `yarn`
- `ember s`

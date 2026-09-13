# JsonApiDotNetCore Todo List Example

Demo application for [JsonApiDotNetCore](https://github.com/json-api-dotnet/JsonApiDotNetCore/) using [Ember.js](https://emberjs.com/).

Back in 2017, Jared Nance did an excellent [video series](https://www.youtube.com/watch?v=KAMuo6K7VcE&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf) in which he built this demo:
- [Part 1: Server Setup](https://www.youtube.com/watch?v=KAMuo6K7VcE&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf)
- [Part 2: Client Setup](https://www.youtube.com/watch?v=_d53rG2i9pY&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf&index=2)
- [Part 3: Server Authentication and Authorization](https://www.youtube.com/watch?v=GIQqIz1Gpvo&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf&index=4)
- [Part 4: Client Sessions](https://www.youtube.com/watch?v=CHdoya6rvaA&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf&index=6)
- [Part 5: Persisting Data](https://www.youtube.com/watch?v=bZ1D_aYGJnU&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf&index=7)

## Usage

### Start the database

The app requires a running PostgreSQL instance with credentials specified in `appsettings.json`.
One way to do this is to run the database in a Docker container:

```shell
docker run --name TodoListSampleDb -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=TodoList -p 5432:5432 -d postgres
```

### Starting the API

- Switch directory:
  - `cd TodoListAPI`
- Set the ASP.NET environment to Development:
  - PowerShell (Windows): `$env:ASPNETCORE_ENVIRONMENT="Development"`
  - Bash (Linux / macOS): `export ASPNETCORE_ENVIRONMENT=Development`
- Start the server:
  - `dotnet run`

The API will run on `http://localhost:5000/`.

### Starting the Client

The client is built with Ember.js and requires Node.js.

- Switch directory:
  - `cd TodoListClient`

- Restore packages:
  - `npm install` (or `yarn install`)

- Start the client:
  - `npm start` (or `ember serve`)

- Open [http://localhost:4200/](http://localhost:4200/) in your browser.

In case you haven't watched the videos, the default username/password is `guest`/`Guest1!`.

## Running Client Tests and Linting

From the `TodoListClient` directory:

- Run all linters and tests:
  - `npm test`
- Run only Ember tests:
  - `npm run test:ember` (or `ember test`)
- Run Ember tests in interactive/server mode:
  - `ember test --server`
- Run code linters:
  - `npm run lint`
- Automatically fix lint and format issues:
  - `npm run lint:fix`

## Updating to the latest version of Ember

Usually the following commands are sufficient:
- `npm install -g ember-cli-update`
- `ember-cli-update`
- `ember-cli-update --run-codemods`
- `npm install`

### Testing if everything still works
- application starts and displays login form
- login with invalid username/password shows popup
- login with correct username/password shows single todo-item "owned-by-guest"
- input validation: adding a todo-item with fewer than 4 characters displays a validation error
- after adding a todo-item, you're taken back to the list, which includes the new item
- clicking logout takes you back to the login page
- navigating to [http://localhost:4200/s/todo-items](http://localhost:4200/s/todo-items) when logged out redirects to the login page

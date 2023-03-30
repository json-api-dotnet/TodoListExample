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

The app requires running postgres instance with credentials specified in appsettings.json.
One way to do this is run the database in a Docker container:

```sh
docker run --name TodoListSampleDb \
    -e POSTGRES_USER=postgres \
    -e POSTGRES_PASSWORD=postgres \
    -e POSTGRES_DB=TodoList \
    -p 5432:5432 \
    -d postgres
```

### Starting the API

- Switch directory
  - `cd TodoListAPI`
- Set the environment to development (mac)
  - `export ASPNETCORE_ENVIRONMENT=development`
- Start the server
  - `dotnet run`

### Starting the Client

- Switch directory
  - `cd TodoListClient`

- Install ember-cli
  - `npm install -g ember-cli`

- Restore packages
  - `npm install -g yarn`
  - `yarn install`

- Start the client
  - `yarn start`
  or
  - `ember s` 
  
- Open http://localhost:4200/ in your browser

In case you haven't watched the videos: the default username/password is `guest`/`Guest1!`.

## Updating to the latest version of Ember

Usually the following commands are sufficient
- `npm install -g ember-cli-update`
- `ember-cli-update`
- `ember-cli-update --run-codemods`
- `yarn install`

### Testing if everything still works
- application starts and displays login link
- login with invalid username/password shows popup
- login with correct username/password shows single todo-item "owned-by-guest"
- input validation: adding a todo-item with less than 4 characters is not possible
- after adding a todo-item, you're taken back to the list, which includes the new item
- clicking logout takes you back to the login page
- navigating to http://localhost:4200/s/todo-items when logged out takes you to the login page

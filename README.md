# JsonApiDotNetCore Todo List Example

A full-stack sample application demonstrating how [Ember.js](https://emberjs.com/) and [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) work together seamlessly using [OpenIddict](https://documentation.openiddict.com/) for authentication and token refresh, and [JsonApiDotNetCore](https://github.com/json-api-dotnet/JsonApiDotNetCore/) for full [JSON:API](https://jsonapi.org/) specification compliance.

Back in 2017, Jared Nance created an excellent [video series](https://www.youtube.com/watch?v=KAMuo6K7VcE&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf) building the initial version of this demo:
- [Part 1: Server Setup](https://www.youtube.com/watch?v=KAMuo6K7VcE&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf)
- [Part 2: Client Setup](https://www.youtube.com/watch?v=_d53rG2i9pY&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf&index=2)
- [Part 3: Server Authentication and Authorization](https://www.youtube.com/watch?v=GIQqIz1Gpvo&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf&index=4)
- [Part 4: Client Sessions](https://www.youtube.com/watch?v=CHdoya6rvaA&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf&index=6)
- [Part 5: Persisting Data](https://www.youtube.com/watch?v=bZ1D_aYGJnU&list=PLu4Bq53iqJJAo1RF0TY4Q5qCG7n9AqSZf&index=7)

This repository has since been updated to modern **.NET 10**, **Ember.js 6.12 LTS** (built with **Embroider + Vite**), **Ember Data 5.3 LTS**, and **OpenIddict 7**.

---

## Prerequisites

Before getting started, make sure you have the following installed:

- **[.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)** (`dotnet --version` should return `10.0.x`)
- **[Node.js](https://nodejs.org/)** (`>= 20.19.0`, LTS recommended) and **npm**
- **[Docker](https://www.docker.com/)** (or an existing [PostgreSQL](https://www.postgresql.org/) instance)
- **[Google Chrome](https://www.google.com/chrome/)** (required by Testem for running Ember acceptance tests)
- *Optional:* **[Visual Studio Code](https://code.visualstudio.com/)** (with the C# Dev Kit extension), Visual Studio 2026, or JetBrains Rider.

---

## Pre-Seeded Demo Accounts

When the API runs for the first time, it automatically creates the PostgreSQL database and seeds two demo user accounts:

| Username | Password | Email | Initial Seeded Item |
| :--- | :--- | :--- | :--- |
| `guest` | `Guest1!` | `guest@email.com` | `"owned-by-guest"` |
| `john` | `P@ssw0rd!` | `john@email.com` | `"owned-by-john"` |

**User Data Isolation:** The API enforces user tenancy through `TodoItemDefinition`. When logging in as `guest`, you will only see and manage todo-items owned by `guest`. When logging in as `john`, you will only see and manage items owned by `john`.

---

## Getting Started

### 1. Start the Database

The application expects a PostgreSQL instance with the connection details defined in `TodoListAPI/appsettings.json`. You can start it using Docker Compose:

```shell
docker compose up --detach --wait
```

- This starts a PostgreSQL database container and a **pgAdmin** container (accessible at [http://localhost:5050](http://localhost:5050), preconfigured to automatically connect to the database).
- To stop the containers:
  ```shell
  docker compose down
  ```
- To stop the containers and remove persistent data (reset the database):
  ```shell
  docker compose down --volumes
  ```

> [!NOTE]
> The API will automatically create database tables and seed the demo users and todo-items on its first run.

### 2. Start the API

```shell
cd TodoListAPI
dotnet run
```

- The API reads development settings from `Properties/launchSettings.json` and starts listening on:
  - `http://localhost:5000/`
  - `https://localhost:44344/`
- You can test API endpoints directly in your editor using the preconfigured [TodoListAPI.http](TodoListAPI/TodoListAPI.http) file.

### 3. Start the Client

In a separate terminal window:

```shell
cd TodoListClient
npm install
npm start
```

- Open [http://localhost:4200/](http://localhost:4200/) in your browser.
- Sign in using either the `guest` or `john` credentials listed above.

---

## Testing Token Refresh & Expiration

The API uses **OpenIddict** configured with OAuth2 password grant and refresh token flows (`connect/token`). By default, tokens have long lifetimes in development.

To observe token expiration and automatic background token refresh in action:

1. Open `TodoListAPI/Program.cs` and uncomment these two lines:
   ```csharp
   options.SetAccessTokenLifetime(TimeSpan.FromSeconds(15));
   options.SetRefreshTokenLifetime(TimeSpan.FromSeconds(30));
   ```
2. Restart the API (`dotnet run`).
3. In your browser, log in to the Ember app ([http://localhost:4200](http://localhost:4200)) and open Developer Tools (**F12** &rarr; **Network** tab).
4. Filter network traffic by `token`.
5. Wait ~15 seconds: **Ember Simple Auth** automatically detects that the access token is expiring and transparently sends a `POST /connect/token` request with `grant_type=refresh_token` to retrieve a fresh token. The user session stays active without interruption or forced re-login.

Alternatively, you can test the refresh token flow without the UI using `TodoListAPI/TodoListAPI.http`.

---

## Running Client Tests and Linting

From the `TodoListClient` directory:

- **Run all linters and tests:**
  ```shell
  npm test
  ```
- **Run Ember acceptance tests headlessly:**
  ```shell
  npm run test:ember
  ```
- **Run Ember tests interactively (in-browser with live reload):**
  Start the dev server (`npm start`) and navigate to [http://localhost:4200/tests](http://localhost:4200/tests).
- **Run code linters (ESLint, Prettier, Stylelint, Template-Lint):**
  ```shell
  npm run lint
  ```
- **Automatically fix lint and formatting issues:**
  ```shell
  npm run lint:fix
  ```
- **Build production assets:**
  ```shell
  npm run build
  ```

---

## Running End-to-End (E2E) Tests

The repository includes an end-to-end test suite built with [Playwright](https://playwright.dev/) that validates the entire stack against the real PostgreSQL database, ASP.NET Core API server, and Ember.js client app.

### Running E2E Tests Locally (using Docker)

1. **Start the database in Docker:**
   ```shell
   docker compose up --detach --wait
   ```

2. **From the `TodoListClient` directory, install browsers (one-time setup):**
   ```shell
   cd TodoListClient
   npx playwright install chromium
   ```

3. **Run the tests:**
   ```shell
   npm run test:e2e
   ```

> [!TIP]
> **Automatic Server Management:** If the backend API (`dotnet run`) and frontend client (`npm start`) are already running, Playwright reuses them automatically. If they are not running, Playwright launches them in the background, runs the test suite, and shuts them down upon completion.

You can also run tests interactively with the Playwright UI runner:

```shell
npx playwright test --ui
```

---

## Updating Ember.js

The client project uses standard npm packaging and is configured with Ember's modern Vite blueprint (`@ember/app-blueprint`). To upgrade Ember dependencies in the future, use `npx ember-cli-update`:

1. Run the update tool:
   ```shell
   npx ember-cli-update
   ```
2. Review and resolve any merge conflicts in project configuration files.
3. Run applicable codemods:
   ```shell
   npx ember-cli-update --run-codemods
   ```
4. Reinstall dependencies:
   ```shell
   npm install
   ```
5. Run tests to confirm everything still passes:
   ```shell
   npm test
   ```

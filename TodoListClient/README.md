# TodoListClient

Modern [Ember.js](https://emberjs.com/) front-end for the Todo List example application, built with [Embroider](https://github.com/embroider-build/embroider) and [Vite](https://vite.dev/). Demonstrates authentication and token refresh with [OpenIddict](https://documentation.openiddict.com/) and JSON:API specification compliance with [JsonApiDotNetCore](https://github.com/json-api-dotnet/JsonApiDotNetCore/).

## Prerequisites

You will need the following installed on your computer:

- [Git](https://git-scm.com/)
- [Node.js](https://nodejs.org/) (version `>= 20.19.0`, LTS recommended) and npm
- [Google Chrome](https://google.com/chrome/) (required for running headless tests)

## Installation

```shell
cd TodoListClient
npm install
```

## Running / Development

Start the Vite development server:

```shell
npm start
```

- Visit the application at [http://localhost:4200](http://localhost:4200).
- Run and view interactive in-browser tests with hot module reloading at [http://localhost:4200/tests](http://localhost:4200/tests).

## Running Tests

From the `TodoListClient` directory:

- Run all linters and tests:
  ```shell
  npm test
  ```
- Run Ember acceptance tests headlessly:
  ```shell
  npm run test:ember
  ```
- Run tests interactively in the browser:
  Start the dev server (`npm start`) and navigate to [http://localhost:4200/tests](http://localhost:4200/tests).
- Run end-to-end tests against the real API (requires PostgreSQL running in Docker):
  ```shell
  npm run test:e2e
  ```

## Linting & Formatting

- Check code style and linting (JavaScript, templates, CSS, and Prettier formatting):
  ```shell
  npm run lint
  ```
- Automatically fix lint and formatting issues:
  ```shell
  npm run lint:fix
  ```

## Building

- Production build:
  ```shell
  npm run build
  ```
  _(compiles optimized assets into the `dist/` directory using Vite)_
- Development build:
  ```shell
  npx vite build --mode development
  ```

## Upgrading Ember

The client is configured with Ember's modern Vite blueprint (`@ember/app-blueprint`). To upgrade dependencies in the future:

```shell
npx ember-cli-update
```

## Further Reading / Useful Links

- [Ember.js](https://emberjs.com/)
- [Embroider](https://github.com/embroider-build/embroider)
- [Vite](https://vite.dev/)
- [Ember CLI Guides](https://cli.emberjs.com/release/)
- Development Browser Extensions:
  - [Ember Inspector for Chrome](https://chrome.google.com/webstore/detail/ember-inspector/bmdblncegkenkacieihfhpjfppoconhi)
  - [Ember Inspector for Firefox](https://addons.mozilla.org/en-US/firefox/addon/ember-inspector/)

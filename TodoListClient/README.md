# TodoListClient

Ember.js front-end for the Todo List example application, demonstrating authentication with OpenIddict and JSON:API communication with JsonApiDotNetCore.

## Prerequisites

You will need the following installed on your computer:

- [Git](https://git-scm.com/)
- [Node.js](https://nodejs.org/) (version `>= 20.19.0`) and npm
- [Google Chrome](https://google.com/chrome/) (required for running tests)

## Installation

```shell
cd TodoListClient
npm install
```

## Running / Development

Start the development server:

```shell
npm start
```

- Visit the application at [http://localhost:4200](http://localhost:4200).
- Visit the in-browser tests at [http://localhost:4200/tests](http://localhost:4200/tests).

## Running Tests

From the `TodoListClient` directory:

- Run all linters and tests:
  ```shell
  npm test
  ```
- Run only the Ember tests:
  ```shell
  npm run test:ember
  ```
- Run tests in interactive/server mode (re-runs on file changes):
  ```shell
  npx ember test --server
  ```

## Linting & Formatting

- Check code style and linting (JavaScript, templates, CSS, Prettier):
  ```shell
  npm run lint
  ```
- Automatically fix lint and formatting issues:
  ```shell
  npm run lint:fix
  ```

## Building

- Development build:
  ```shell
  npx ember build
  ```
- Production build:
  ```shell
  npm run build
  ```

## Further Reading / Useful Links

- [Ember.js](https://emberjs.com/)
- [Ember CLI Guides](https://cli.emberjs.com/release/)
- Development Browser Extensions:
  - [Ember Inspector for Chrome](https://chrome.google.com/webstore/detail/ember-inspector/bmdblncegkenkacieihfhpjfppoconhi)
  - [Ember Inspector for Firefox](https://addons.mozilla.org/en-US/firefox/addon/ember-inspector/)

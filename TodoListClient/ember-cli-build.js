'use strict';

const EmberApp = require('ember-cli/lib/broccoli/ember-app');
const { setConfig } = require('@warp-drive/build-config');

module.exports = function (defaults) {
  let app = new EmberApp(defaults, {
    'ember-simple-auth': {
      useSessionSetupMethod: true,
    },
  });

  app.import('node_modules/bootstrap/dist/css/bootstrap.min.css');

  setConfig(app, __dirname, {
    deprecations: {
      DEPRECATE_STORE_EXTENDS_EMBER_OBJECT: false,
    },
  });

  return app.toTree();
};

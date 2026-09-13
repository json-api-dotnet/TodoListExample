import { createServer, Model, Factory, JSONAPISerializer } from 'miragejs';

export function startMirage({ environment = 'test' } = {}) {
  return createServer({
    environment,
    models: {
      todoItem: Model.extend(),
    },
    factories: {
      todoItem: Factory.extend({
        description: (i) => `Description ${i}`,
      }),
    },
    serializers: {
      application: JSONAPISerializer.extend({}),
    },
    routes() {
      this.urlPrefix = 'http://localhost:5000';
      this.get('/api/v1/todo-items', 'todo-items');
    },
  });
}

export function setupMirage(hooks) {
  hooks.beforeEach(function () {
    this.server = startMirage({ environment: 'test' });
  });

  hooks.afterEach(function () {
    this.server.shutdown();
  });
}

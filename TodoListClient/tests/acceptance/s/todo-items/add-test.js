import { module, test } from 'qunit';
import { setupApplicationTest } from 'ember-qunit';
import { currentURL, click, fillIn, visit } from '@ember/test-helpers';
import { setupMirage } from 'ember-cli-mirage/test-support';
import {
  authenticateSession,
  invalidateSession,
} from 'ember-simple-auth/test-support';

module('Acceptance | S | Todo Items | Add', (hooks) => {
  setupApplicationTest(hooks);
  setupMirage(hooks);

  test('redirects to login if not authenticated', async function (assert) {
    assert.expect(1);

    await invalidateSession();
    await visit('/s/todo-items/add');

    assert.strictEqual(currentURL(), '/login', 'the route is correct');
  });

  test('can submit valid jsonapi request', async function (assert) {
    assert.expect(4);

    await authenticateSession();
    await visit('/s/todo-items');
    await click('[data-test-add]');

    assert.strictEqual(
      currentURL(),
      '/s/todo-items/add',
      'the route is correct'
    );

    this.server.post('/api/v1/todo-items', ({ todoItems }, request) => {
      const body = JSON.parse(request.requestBody);
      const expectedRequest = {
        data: {
          attributes: {
            description: 'FooBar',
          },
          type: 'todo-items',
        },
      };

      assert.deepEqual(expectedRequest, body, 'request is correctly formatted');

      return todoItems.create();
    });

    await fillIn('[data-test-description-input]', '123');
    await click('[data-test-submit]');

    assert.strictEqual(
      currentURL(),
      '/s/todo-items/add',
      'stays on route after failed save'
    );

    await fillIn('[data-test-description-input]', 'FooBar');
    await click('[data-test-submit]');

    assert.strictEqual(currentURL(), '/s/todo-items', 'redirects after save');
  });
});

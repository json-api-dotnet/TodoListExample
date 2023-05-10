import { module, test } from 'qunit';
import { setupApplicationTest } from 'ember-qunit';
import {
  currentURL,
  click,
  find,
  fillIn,
  waitFor,
  visit,
} from '@ember/test-helpers';
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
    assert.expect(3);

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

    await fillIn('[data-test-description-input]', 'FooBar');
    await click('[data-test-submit]');

    assert.strictEqual(currentURL(), '/s/todo-items', 'redirects after save');
  });

  test('empty descriptions will error', async function (assert) {
    assert.expect(2);

    await authenticateSession();
    await visit('/s/todo-items');
    await click('[data-test-add]');

    this.server.post('/api/v1/todo-items', () => {
      assert.step('should not run');
    });

    click('[data-test-submit]');

    await waitFor('.message', { timeout: 2000 }).then(() => {
      assert.strictEqual(
        find('.message').textContent,
        "Description can't be blank ",
        'error message is displayed'
      );
    });

    assert.strictEqual(
      currentURL(),
      '/s/todo-items/add',
      'stays on route after failed save'
    );
  });

  test('short descriptions will error', async function (assert) {
    assert.expect(2);

    await authenticateSession();
    await visit('/s/todo-items');
    await click('[data-test-add]');

    this.server.post('/api/v1/todo-items', () => {
      assert.step('should not run');
    });

    await fillIn('[data-test-description-input]', '123');
    click('[data-test-submit]');

    await waitFor('.message', { timeout: 2000 }).then(() => {
      assert.strictEqual(
        find('.message').textContent,
        'Description is too short (minimum is 4 characters) ',
        'error message is displayed'
      );
    });

    assert.strictEqual(
      currentURL(),
      '/s/todo-items/add',
      'stays on route after failed save'
    );
  });
});

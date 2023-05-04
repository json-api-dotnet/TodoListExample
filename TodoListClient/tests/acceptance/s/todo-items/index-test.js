import { module, test } from 'qunit';
import { setupApplicationTest } from 'ember-qunit';
import { currentURL, findAll, visit } from '@ember/test-helpers';
import { setupMirage } from 'ember-cli-mirage/test-support';
import {
  authenticateSession,
  invalidateSession,
} from 'ember-simple-auth/test-support';

module('Acceptance | S | Todo Items | Index', (hooks) => {
  setupApplicationTest(hooks);
  setupMirage(hooks);

  test('redirects to login if not authenticated', async function (assert) {
    assert.expect(1);

    await invalidateSession();
    await visit('/s/todo-items');

    assert.strictEqual(currentURL(), '/login', 'the route is correct');
  });

  test("shows list of user's todo items", async function (assert) {
    assert.expect(7);

    const items = this.server.createList('todo-item', 5);

    await authenticateSession();
    await visit('/s/todo-items');

    assert.strictEqual(currentURL(), '/s/todo-items', 'the route is correct');

    const itemRows = findAll('[data-test-item]');

    assert.strictEqual(
      itemRows.length,
      items.length,
      'All items are displayed'
    );

    itemRows.forEach((row, index) => {
      let item = items[index];

      assert.strictEqual(
        row.querySelector('[data-test-description]').textContent,
        item.description,
        'Description is displayed for each item'
      );
    });
  });
});

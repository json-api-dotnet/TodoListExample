import { module, test } from 'qunit';
import { setupApplicationTest } from 'ember-qunit';
import { currentURL, click, fillIn, visit } from '@ember/test-helpers';
import { setupMirage } from 'ember-cli-mirage/test-support';
import {
  authenticateSession,
  invalidateSession,
} from 'ember-simple-auth/test-support';

module('Acceptance | Login', (hooks) => {
  setupApplicationTest(hooks);
  setupMirage(hooks);

  test('redirects to index if authenticated', async function (assert) {
    assert.expect(1);

    await authenticateSession();
    await visit('/login');

    assert.strictEqual(currentURL(), '/s/todo-items', 'the route is correct');
  });

  test('can submit valid login creds', async function (assert) {
    assert.expect(4);

    await invalidateSession();
    await visit('/login');

    assert.strictEqual(currentURL(), '/login', 'the route is correct');

    this.server.post('/connect/token', (_, request) => {
      const params = request.requestBody.split('&').reduce((result, part) => {
        const [key, value] = part.split('=');
        result[key] = decodeURIComponent(value);

        return result;
      }, {});

      assert.strictEqual(params.username, 'guest', 'username is correct');
      assert.strictEqual(params.password, 'Guest1!', 'password is correct');

      return {
        access_token: 'a very secret token',
        token_type: 'Bearer',
      };
    });

    await fillIn('[data-test-username]', 'guest');
    await fillIn('[data-test-password]', 'Guest1!');
    await click('[data-test-submit]');

    assert.strictEqual(currentURL(), '/s/todo-items', 'redirects after save');
  });
});

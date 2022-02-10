import { module, test } from 'qunit';
import { setupTest } from 'ember-qunit';

module('Unit | Route | todo items', function (hooks) {
  setupTest(hooks);

  test('it exists', function (assert) {
    let route = this.owner.lookup('route:todo-items');
    assert.ok(route);
  });
});

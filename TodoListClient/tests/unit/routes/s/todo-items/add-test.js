import { module, test } from 'qunit';
import { setupTest } from 'ember-qunit';

module('Unit | Route | s/todo items/add', function(hooks) {
  setupTest(hooks);

  test('it exists', function(assert) {
    let route = this.owner.lookup('route:s/todo-items/add');
    assert.ok(route);
  });
});

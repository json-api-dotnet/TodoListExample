import { module, test } from 'qunit';
import { setupRenderingTest } from 'ember-qunit';
import { render } from '@ember/test-helpers';
import hbs from 'htmlbars-inline-precompile';

module('Integration | Component | forms/todo item form', function(hooks) {
  setupRenderingTest(hooks);

  test('it renders', async function(assert) {

    // Set any properties with this.set('myProperty', 'value');
    // Handle any actions with this.on('myAction', function(val) { ... });

    await render(hbs`{{forms/todo-item-form}}`);

    assert.dom(this.element).hasText('');

    // Template block usage:
    await render(hbs`
      {{#forms/todo-item-form}}
        template block text
      {{/forms/todo-item-form}}
    `);

    assert.dom(this.element).hasText('template block text');
  });
});

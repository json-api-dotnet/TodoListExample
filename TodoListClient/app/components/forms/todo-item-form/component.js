import Component from '@glimmer/component';
import { action } from '@ember/object';
import { inject as service } from '@ember/service';
import Changeset from 'ember-changeset';
import lookupValidator from 'ember-changeset-validations';
import TodoItemValidations from '../../../validations/todo-item';

export default class TodoItemForm extends Component {
  @service notify;
  @service router;

  constructor() {
    super(...arguments);

    // Was unable to use the changeset helpr, so I'm constructing the changeset manually.
    // Most likely related to this issue, but the fix didn't work.
    // https://github.com/poteto/ember-changeset-validations/issues/214
    this.changeset = new Changeset(
      this.args.todoItem,
      lookupValidator(TodoItemValidations),
      TodoItemValidations
    );
  }

  @action
  async save(event) {
    event.preventDefault();
    try {
      await this.changeset.validate();

      if (this.changeset.isValid) {
        await this.changeset.save();
        this.router.transitionTo('s.todo-items');
      } else {
        const errors = this.changeset.errors.map(error => `${error.validation} <br />`).reduce((accumulator, currentValue) => accumulator + currentValue);
        this.notify.error({ html: errors });
      }
    } catch (error) {
      this.notify.error('Failed to create item');
    }
  }
}

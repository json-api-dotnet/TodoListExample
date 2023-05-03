import Component from '@glimmer/component';
import { action } from '@ember/object';
import { inject as service } from '@ember/service';

export default class TodoItemForm extends Component {
  @service notify;
  @service router;

  @action
  async save(event) {
    event.preventDefault();
    try {
      await this.args.changeset.validate();

      if (this.args.changeset.isValid) {
        await this.args.changeset.save();
        this.router.transitionTo('s.todo-items');
      } else {
        const errors = this.args.changeset.errors
          .map((error) => `${error.validation} <br />`)
          .reduce((accumulator, currentValue) => accumulator + currentValue);
        this.notify.error({ html: errors });
      }
    } catch (error) {
      this.notify.error('Failed to create item');
    }
  }
}

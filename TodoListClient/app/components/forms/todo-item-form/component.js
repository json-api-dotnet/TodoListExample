import Component from '@glimmer/component';
import { action } from '@ember/object';
import { inject as service } from '@ember/service';

export default class TodoItemForm extends Component {
  @service notify;
  @service router;

  @action
  async save(todoItem, event) {
    event.preventDefault();
    try {
      await todoItem.save();
      this.router.transitionTo('s.todo-items');
    } catch(error) {
      this.notify.error('Failed to create item');
    }
  }
}
import Controller from '@ember/controller';
import TodoItemValidations from '../../../validations/todo-item';

export default class AddController extends Controller {
  TodoItemValidations = TodoItemValidations;
}

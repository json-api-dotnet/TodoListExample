import Model, { attr } from '@ember-data/model';

export default class TodoItemModel extends Model {
  @attr('string') description;
}

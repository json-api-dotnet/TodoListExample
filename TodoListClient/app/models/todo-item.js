import Model, { belongsTo, attr } from '@ember-data/model';

export default class TodoItemModel extends Model {
  @attr description;
  @belongsTo('person', { async: true, inverse: null }) person;
}

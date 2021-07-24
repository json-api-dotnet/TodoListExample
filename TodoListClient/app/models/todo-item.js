import Model, { belongsTo, attr } from '@ember-data/model';

export default class TodoItem extends Model {
  @attr description;
  @belongsTo person;
}

import Model, { hasMany, attr } from '@ember-data/model';

export default Model.extend({

  firstName: attr('string'),
  lastName: attr('string'),
  todoItems: hasMany('todo-item')

});

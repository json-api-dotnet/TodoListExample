import Route from '@ember/routing/route';
import DataRoute from 'ember-data-route';

export default Route.extend(DataRoute, {
  model() {
    return this.store.createRecord('todo-item');
  },

  actions: {
    save() {
      this.get('controller.model')
        .save()
        .then(() => {
          this.transitionTo('s.todo-items');
        });
    }
  },

});

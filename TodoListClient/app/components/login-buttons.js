import Ember from 'ember';

const { service } = Ember.inject;

export default Ember.Component.extend({

  session: service(),

  actions: {
    invalidateSession() {
      this.get('session').invalidate();
    }
  },

});

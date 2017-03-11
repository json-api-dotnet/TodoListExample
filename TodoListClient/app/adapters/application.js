import DS from 'ember-data';
import ENV from 'todo-list-client/config/environment';

const { computed, inject: { service } } = Ember;

export default DS.JSONAPIAdapter.extend({
  session: service(),

  namespace: ENV.APP.namespace,
  host: ENV.APP.host,

  headers: computed('session.data.authenticated.token', function() {
    let token = this.get('session.data.authenticated.access_token');
    return { Authorization: `Bearer ${token}` };
  }),


});

import JSONAPIAdapter from '@ember-data/adapter/json-api';
import { computed } from '@ember/object';
import { inject as service } from '@ember/service';
import ENV from 'todo-list-client/config/environment';

export default JSONAPIAdapter.extend({
  session: service(),

  namespace: ENV.APP.namespace,
  host: ENV.APP.host,

  headers: computed('session.data.authenticated.token', function() {
    let token = this.get('session.data.authenticated.access_token');
    return { Authorization: `Bearer ${token}` };
  }),


});

import JSONAPIAdapter from '@ember-data/adapter/json-api';
import { service } from '@ember/service';
import ENV from 'todo-list-client/config/environment';

export default class ApplicationAdapter extends JSONAPIAdapter {
  @service session;

  namespace = ENV.APP.namespace;
  host = ENV.APP.host;

  get headers() {
    let token = this.session.data.authenticated.access_token;
    return { Authorization: `Bearer ${token}` };
  }
}

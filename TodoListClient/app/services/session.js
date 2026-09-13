import SessionService from 'ember-simple-auth/services/session';
import Configuration from 'ember-simple-auth/configuration';
import SessionStore from '../session-stores/application';
import EphemeralStore from 'ember-simple-auth/session-stores/ephemeral';
import OAuth2Authenticator from '../authenticators/oauth2';
import config from 'todo-list-client/config/environment';

export default class Session extends SessionService {
  createConfiguration() {
    return Configuration.load({
      useResolver: false,
      rootURL: '/',
    });
  }

  createSessionStore(owner) {
    if (config.environment === 'test') {
      return new EphemeralStore(owner);
    }
    return new SessionStore(owner);
  }

  createAuthenticators(owner) {
    return [new OAuth2Authenticator(owner)];
  }
}

import Route from '@ember/routing/route';
import { inject as service } from '@ember/service';

export default class S extends Route {
  @service session;

  beforeModel(transition) {
    if (!this.session.isAuthenticated) {
      this.session.requireAuthentication(transition, 'login');
    }
  }
}

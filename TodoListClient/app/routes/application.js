import Route from '@ember/routing/route';
import { inject as service } from '@ember/service';

export default class ApplicationRoute extends Route {
  @service session;

  beforeModel(/* transition */) {
    if (this.session.isAuthenticated) {
      this.transitionTo('s.todo-items');
    }
  }
}

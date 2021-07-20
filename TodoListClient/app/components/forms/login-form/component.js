import Component from '@glimmer/component';
import { action } from '@ember/object';
import { inject as service } from '@ember/service';
import { tracked } from '@glimmer/tracking';

export default class LoginForm extends Component {
  @service session;
  @service notify;
  @service router;

  @tracked identification;
  @tracked password;

  @action
  async authenticate(event) {
    event.preventDefault();
    try {
      await this.session.authenticate('authenticator:oauth2', this.identification, this.password);
      this.router.transitionTo('s.todo-items');
    } catch(error) {
      this.notify.error('Authentication failed');
    }
  }
}
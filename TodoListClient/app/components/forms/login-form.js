import Component from '@glimmer/component';
import { action } from '@ember/object';
import { service } from '@ember/service';
import { tracked } from '@glimmer/tracking';

export default class LoginForm extends Component {
  @service session;
  @service notify;
  @service router;

  @tracked identification;
  @tracked password;

  @action
  updateIdentification(event) {
    this.identification = event.target.value;
  }

  @action
  updatePassword(event) {
    this.password = event.target.value;
  }

  @action
  async authenticate(event) {
    event.preventDefault();
    try {
      await this.session.authenticate(
        'authenticator:oauth2',
        this.identification,
        this.password,
      );
      this.router.transitionTo('s.todo-items');
    } catch {
      this.notify.error('Authentication failed');
    }
  }
}

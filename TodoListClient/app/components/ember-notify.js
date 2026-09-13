import Component from '@glimmer/component';
import { service } from '@ember/service';
import { action } from '@ember/object';

export default class EmberNotifyComponent extends Component {
  @service notify;

  @action
  close(message) {
    this.notify.remove(message);
  }
}

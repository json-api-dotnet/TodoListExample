import Service from '@ember/service';
import { tracked } from '@glimmer/tracking';
import { htmlSafe } from '@ember/template';

export default class NotifyService extends Service {
  @tracked messages = [];

  error(message, options = {}) {
    return this.show(message, { type: 'danger', ...options });
  }

  info(message, options = {}) {
    return this.show(message, { type: 'info', ...options });
  }

  success(message, options = {}) {
    return this.show(message, { type: 'success', ...options });
  }

  warning(message, options = {}) {
    return this.show(message, { type: 'warning', ...options });
  }

  show(message, options = {}) {
    const text = typeof message === 'string' ? message : message?.text || '';
    const html =
      typeof message === 'object' && message?.html
        ? htmlSafe(message.html)
        : null;
    const item = {
      id: Math.random().toString(36).slice(2),
      text,
      html,
      type: options.type || 'info',
      closeAfter: options.closeAfter ?? 2500,
    };

    this.messages = [...this.messages, item];

    if (item.closeAfter > 0) {
      setTimeout(() => {
        this.remove(item);
      }, item.closeAfter);
    }

    return item;
  }

  remove(item) {
    this.messages = this.messages.filter((m) => m !== item);
  }
}

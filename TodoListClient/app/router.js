import EmberRouter from '@ember/routing/router';
import config from 'todo-list-client/config/environment';

export default class Router extends EmberRouter {
  location = config.locationType;
  rootURL = config.rootURL;
}

Router.map(function() {
  this.route('login');
  this.route('s', function() {
    this.route('todo-items', function() {
      this.route('add');
    });
  });
});

import Ember from 'ember';
import config from './config/environment';

const Router = Ember.Router.extend({
  location: config.locationType,
  rootURL: config.rootURL
});

Router.map(function() {
  this.route('login');
  this.route('s', function() {
    this.route('todo-items', function() {
      this.route('add');
    });
  });
});

export default Router;

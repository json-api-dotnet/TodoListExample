import Ember from 'ember';

const { service } = Ember.inject;

export default Ember.Component.extend({

  session: service(),
  notify: service(),

  actions: {
    authenticate() {
      let { identification, password } = this.getProperties('identification', 'password');
      this.get('session').authenticate('authenticator:oauth2', identification, password).catch((reason) => {
        this.get('notify').error('Authentication failed');
      });
    }
  },

});

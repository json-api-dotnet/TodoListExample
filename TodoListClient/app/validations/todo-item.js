import {
  validatePresence,
  validateLength,
} from 'ember-changeset-validations/validators';

export default {
  description: [validatePresence(true), validateLength({ min: 4 })],
};

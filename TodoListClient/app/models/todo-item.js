import Model, { belongsTo, attr } from '@ember-data/model';
import { validator, buildValidations } from 'ember-cp-validations';

const Validations = buildValidations({
  description: [
    validator('presence', true),
    validator('length', {
      min: 4
    })
  ]
});

export default Model.extend(Validations, {

  description: attr('string'),
  owner: belongsTo('person')

});

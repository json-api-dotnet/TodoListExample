import DS from 'ember-data';
import { validator, buildValidations } from 'ember-cp-validations';

const { attr, belongsTo } = DS;
const Validations = buildValidations({
  description: [
    validator('presence', true),
    validator('length', {
      min: 4
    })
  ]
});

export default DS.Model.extend(Validations, {

  description: attr('string'),
  owner: belongsTo('person')

});

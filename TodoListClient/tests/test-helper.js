import Application from 'todo-list-client/app';
import config from 'todo-list-client/config/environment';
import * as QUnit from 'qunit';
import { setApplication } from '@ember/test-helpers';
import { setup } from 'qunit-dom';
import { start as qunitStart, setupEmberOnerrorValidation } from 'ember-qunit';

export function start() {
  setApplication(Application.create(config.APP));

  QUnit.config.countStepsAsOne = true;

  setup(QUnit.assert);
  setupEmberOnerrorValidation();
  qunitStart();
}

import globals from "globals";
import js from "@eslint/js";

import ember from "eslint-plugin-ember/recommended";
import eslintConfigPrettier from "eslint-config-prettier";
import qunit from "eslint-plugin-qunit";
import n from "eslint-plugin-n";

import babelParser from "@babel/eslint-parser";

const esmParserOptions = {
  ecmaFeatures: { modules: true },
  ecmaVersion: "latest",
  requireConfigFile: false,
  babelOptions: {
    plugins: [
      ["@babel/plugin-proposal-decorators", { legacy: true }],
      ["@babel/plugin-proposal-class-properties", { loose: false }],
    ],
  },
};

export default [
  js.configs.recommended,
  eslintConfigPrettier,
  ember.configs.base,
  ember.configs.gjs,
  {
    ignores: ["dist/", "node_modules/", "coverage/", ".eslintcache", "!**/.*"],
  },
  {
    linterOptions: {
      reportUnusedDisableDirectives: "error",
    },
  },
  {
    files: ["**/*.js"],
    languageOptions: {
      parser: babelParser,
    },
  },
  {
    files: ["**/*.{js,gjs}"],
    languageOptions: {
      parserOptions: esmParserOptions,
      globals: {
        ...globals.browser,
      },
    },
  },
  {
    ...qunit.configs.recommended,
    files: ["tests/**/*-test.{js,gjs}"],
    plugins: {
      qunit,
    },
    rules: {
      "qunit/require-expect": "off",
    },
  },
  /**
   * CJS node files
   */
  {
    ...n.configs["flat/recommended-script"],
    files: [
      "**/*.cjs",
      "config/**/*.js",
      "testem.js",
      "testem*.js",
      ".prettierrc.js",
      "ember-cli-build.js",
    ],
    plugins: {
      n,
    },
    languageOptions: {
      sourceType: "script",
      ecmaVersion: "latest",
      globals: {
        ...globals.node,
      },
    },
  },
  /**
   * ESM node files
   */
  {
    ...n.configs["flat/recommended-module"],
    files: ["**/*.mjs"],
    plugins: {
      n,
    },
    languageOptions: {
      sourceType: "module",
      ecmaVersion: "latest",
      parserOptions: esmParserOptions,
      globals: {
        ...globals.node,
      },
    },
  },
];

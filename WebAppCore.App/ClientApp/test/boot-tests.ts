import "reflect-metadata";
import "zone.js";
import "zone.js/dist/long-stack-trace-zone";
import "zone.js/dist/proxy.js";
import "zone.js/dist/sync-test";
import "zone.js/dist/jasmine-patch";
import "zone.js/dist/async-test";
import "zone.js/dist/fake-async-test";
import * as testing from "@angular/core/testing";
import * as testingBrowser from "@angular/platform-browser-dynamic/testing";

declare var karma: any;
declare var require: any;

karma.loaded = () => {};

testing.getTestBed().initTestEnvironment(
    testingBrowser.BrowserDynamicTestingModule,
    testingBrowser.platformBrowserDynamicTesting()
);

const context = require.context("../", true, /\.spec\.ts$/);

context.keys().map(context);

karma.start();
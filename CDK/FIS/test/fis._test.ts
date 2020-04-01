import { expect as expectCDK, matchTemplate, MatchStyle } from '@aws-cdk/assert';
import * as cdk from '@aws-cdk/core';
import Fis = require('../lib/fis-stack');

// test('Empty Stack', () => {
//     const app = new cdk.App();
//     // WHEN
//     const stack = new Fis.FisStack(app, 'MyTestStack');
//     // THEN
//     expectCDK(stack).to(matchTemplate({
//       "Resources": {}
//     }, MatchStyle.EXACT))
// });

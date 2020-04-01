#!/usr/bin/env node
import 'source-map-support/register';
import * as cdk from '@aws-cdk/core';
import { FisStack } from '../lib/fis-stack';

const app = new cdk.App();
new FisStack(app, 'FisStack');

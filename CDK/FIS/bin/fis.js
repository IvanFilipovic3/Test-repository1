#!/usr/bin/env node
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("source-map-support/register");
const cdk = require("@aws-cdk/core");
const fis_stack_1 = require("../lib/fis-stack");
const app = new cdk.App();
new fis_stack_1.FisStack(app, 'FisStack');
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmlzLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiZmlzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7OztBQUNBLHVDQUFxQztBQUNyQyxxQ0FBcUM7QUFDckMsZ0RBQTRDO0FBRTVDLE1BQU0sR0FBRyxHQUFHLElBQUksR0FBRyxDQUFDLEdBQUcsRUFBRSxDQUFDO0FBQzFCLElBQUksb0JBQVEsQ0FBQyxHQUFHLEVBQUUsVUFBVSxDQUFDLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyIjIS91c3IvYmluL2VudiBub2RlXG5pbXBvcnQgJ3NvdXJjZS1tYXAtc3VwcG9ydC9yZWdpc3Rlcic7XG5pbXBvcnQgKiBhcyBjZGsgZnJvbSAnQGF3cy1jZGsvY29yZSc7XG5pbXBvcnQgeyBGaXNTdGFjayB9IGZyb20gJy4uL2xpYi9maXMtc3RhY2snO1xuXG5jb25zdCBhcHAgPSBuZXcgY2RrLkFwcCgpO1xubmV3IEZpc1N0YWNrKGFwcCwgJ0Zpc1N0YWNrJyk7XG4iXX0=
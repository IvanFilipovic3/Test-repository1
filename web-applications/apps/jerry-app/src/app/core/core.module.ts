import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from 'shared/shared.module';

import { WelcomeComponent } from './welcome/welcome.component';

@NgModule({
    declarations: [
        WelcomeComponent
    ],
    imports: [
        CommonModule,
        SharedModule
    ]
})
export class CoreModule { }

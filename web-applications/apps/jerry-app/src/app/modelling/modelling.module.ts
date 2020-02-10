import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from 'shared/shared.module';
import { ConfigService } from '../services/configService';


import { JtListComponent } from './job-templates/jt-list/jt-list.component';
import { JtFormComponent } from './job-templates/jt-form/jt-form.component';

import { ModellingRoutingModule } from './modelling-routing.module';
import { API_BASE_URL, JobTemplatesProxy } from 'client-proxies/modelling-proxy.module';

export function getBaseURL(config: ConfigService) {
    return config.getConfig('modellingEndPoint');
}

@NgModule({
    declarations: [JtListComponent, JtFormComponent],
    imports: [
        CommonModule,
        ModellingRoutingModule,
        SharedModule
    ],
    providers: [
        JobTemplatesProxy,
        {
            provide: API_BASE_URL,
            useFactory: getBaseURL,
            deps: [ConfigService]
        }
    ]
})
export class ModellingModule { }

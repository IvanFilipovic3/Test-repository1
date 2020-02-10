import { Component, OnDestroy, OnInit } from '@angular/core';

import { JobTemplateData, JobTemplatesProxy } from 'client-proxies/modelling-proxy.module';
import { Subscription } from 'rxjs';

@Component({
    selector: 'jerry-prefix-job-templates-list',
    templateUrl: './jt-list.component.html',
    styleUrls: ['./jt-list.component.scss']
})
export class JtListComponent implements OnInit, OnDestroy {
    dataSource: Array<JobTemplateData> = [];
    columnsToDisplay = ['Name', 'Description'];
    subscription: Subscription

    constructor(private jobTemplateService: JobTemplatesProxy) { }

    ngOnInit() {
        this.getAllItems();
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    getAllItems() {
        this.subscription = this.jobTemplateService.getAllJobTemplates().subscribe(result => { this.dataSource = result; });
    }
}

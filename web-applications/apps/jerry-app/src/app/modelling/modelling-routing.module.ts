import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { JtListComponent } from './job-templates/jt-list/jt-list.component';
import { JtFormComponent } from './job-templates/jt-form/jt-form.component';

const routes: Routes = [
    { path: 'job-templates', component: JtListComponent, data: { title: 'Job Templates' } },
    { path: 'job-template', component: JtFormComponent, data: { title: 'Job Template' } }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ModellingRoutingModule { }

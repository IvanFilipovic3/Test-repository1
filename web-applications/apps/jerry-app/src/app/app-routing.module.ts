import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { WelcomeComponent } from './core/welcome/welcome.component';

// ** should really redirect to a PageNotFoundComponent component
const appRoutes: Routes = [
    {
        path: '',
        children: [
            { path: 'welcome', component: WelcomeComponent, data: { title: 'Welcome' } },
            { path: 'modelling', loadChildren: () => import('./modelling/modelling.module').then(m => m.ModellingModule) },
            { path: '', redirectTo: '/welcome', pathMatch: 'full' },
            { path: '**', redirectTo: '/welcome', pathMatch: 'full' }
        ]
    }
]

@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        RouterModule.forRoot(
            appRoutes,
            {
                enableTracing: true,    // Debugging purposes only
                initialNavigation: 'enabled'
            }
        )
    ],
    exports: [
        RouterModule
    ]
})

export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
    MatButtonModule,
    MatCardModule,
    MatTableModule
    //MatFormFieldModule,
    // MatInputModule,
    // MatSelectModule,
    //MatToolbarModule,
    //MatTooltipModule,
    //MatCheckboxModule,
    //MatListModule
} from '@angular/material';
import { RufShellModule } from '@ruf/shell';

import { PageLayoutComponent } from './page-layout/page-layout.component';


@NgModule({
    declarations: [
        PageLayoutComponent
    ],
    imports: [
        CommonModule,
        HttpClientModule,
        RufShellModule,
        FormsModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatCardModule,
        MatTableModule
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RufShellModule,
        MatButtonModule,
        MatCardModule,
        MatTableModule,
        PageLayoutComponent
    ]
})
export class SharedModule { }

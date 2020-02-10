import { Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'jerry-prefix-cm-page-layout',
    templateUrl: './page-layout.component.html',
    styleUrls: ['./page-layout.component.scss']
})

export class PageLayoutComponent implements OnInit {

    @Input() headerTitle: string;
    @Input() headerDesc: string;
    @Input() headerIcon: string;

    constructor() { }

    ngOnInit() {
    }

}

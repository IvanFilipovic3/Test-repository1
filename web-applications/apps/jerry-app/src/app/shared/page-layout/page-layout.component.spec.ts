import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { SharedModule } from 'shared/shared.module';
import { PageLayoutComponent } from './page-layout.component';

describe('PageLayoutComponent', () => {
    let component: PageLayoutComponent;
    let fixture: ComponentFixture<PageLayoutComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [

                BrowserAnimationsModule,
                SharedModule
            ],
            declarations: []
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(PageLayoutComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});

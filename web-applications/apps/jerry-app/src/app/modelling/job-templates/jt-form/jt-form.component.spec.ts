import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedModule } from 'shared/shared.module';
import { JtFormComponent } from './jt-form.component';

describe('JtFormComponent', () => {
    let component: JtFormComponent;
    let fixture: ComponentFixture<JtFormComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                SharedModule
            ],
            declarations: [JtFormComponent]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(JtFormComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});

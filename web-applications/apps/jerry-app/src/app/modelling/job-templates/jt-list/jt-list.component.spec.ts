import '../../../unit-testing/matchMedia.mock';

import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { defer, Observable } from 'rxjs';

import { SharedModule } from 'shared/shared.module';
import { JtListComponent } from './jt-list.component';
import { IJobTemplateData, JobTemplatesProxy } from 'client-proxies/modelling-proxy.module';


export function fakeAsyncResponse<T>(data: T) {
    return defer(() => Promise.resolve(data));
}

const exampleList: IJobTemplateData[] = [
    { name: "Name1", description: 'Desc1' },
    { name: "Name2", description: 'Desc2' },
    { name: "Name3", description: 'Desc3' }
];

jest.mock('client-proxies/modelling-proxy.module', () => ({
    JobTemplatesProxy: class {
        public getAllJobTemplates(): Observable<IJobTemplateData[]> {
            return fakeAsyncResponse(exampleList);
        }
    }
}));

describe('JtListComponent', () => {
    let component: JtListComponent;
    let fixture: ComponentFixture<JtListComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                SharedModule
            ],
            providers: [
                JobTemplatesProxy
            ],
            declarations: [JtListComponent]
        });
        TestBed.compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(JtListComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });


    it('should create', () => {
        expect(component).toBeTruthy();
    });


    it('table should have 2 entries', async(() => {
        fixture.whenStable().then(() => {
            fixture.detectChanges();

            const tableBody = fixture.debugElement.nativeElement.querySelector('tbody');
            const tableRows = tableBody.querySelectorAll('tr');
            expect(tableRows.length).toBe(3);
        });
    }));
});

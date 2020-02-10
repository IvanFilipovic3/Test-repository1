
import './unit-testing/matchMedia.mock'
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from '@angular/material';
import { Component } from '@angular/core';

import { SharedModule } from 'shared/shared.module';
import { AppComponent } from './app.component';
import { Router, Routes } from '@angular/router';

// Create a dummy component for the routing configuration
@Component({ template: '' })
class TestComponent { }

// Dummy routing to test the navigation of the component
const testRoutes: Routes = [
    {
        path: '',
        children: [
            { path: 'welcome', component: TestComponent, data: { title: 'Welcome' } },
            { path: 'page2', component: TestComponent },
            { path: '', redirectTo: '/welcome', pathMatch: 'full' }
        ]
    }
];

describe('AppComponent', () => {
    let component: AppComponent;
    let fixture: ComponentFixture<AppComponent>;
    let router: Router;
    let titleService: Title;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                BrowserAnimationsModule,
                RouterTestingModule.withRoutes(testRoutes),
                MatSidenavModule,
                SharedModule
            ],
            declarations: [
                AppComponent,
                TestComponent
            ],
            providers: [
                { provide: Title, useClass: Title }
            ]
        }).compileComponents();
        router = TestBed.get(Router);
        titleService = TestBed.get(Title);
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AppComponent);
        component = fixture.debugElement.componentInstance;
        titleService.setTitle(component.title);
        fixture.detectChanges();
    });

    it('should create the app', async(() => {
        expect(component).toBeTruthy();
    }));

    it('should render title in a ruf-banner-brand tag', async(() => {
        const compiled = fixture.debugElement.nativeElement;
        expect(compiled.querySelector('ruf-banner-brand').textContent).toContain(component.title);
    }));

    it('should select and route to welcome URL', async(() => {
        fixture.ngZone.run(() => {
            router.initialNavigation();
            fixture.whenStable().then(() => {
                fixture.detectChanges();
                component.menuSelect('welcome').then(() => {
                    expect(router.url).toEqual('/welcome');
                    expect(component.selectedPath()).toEqual('/welcome');
                    expect(titleService.getTitle()).toContain("Welcome");
                });
            });
        });
    }));

    it('should select page with no title override', async(() => {
        fixture.ngZone.run(() => {
            router.initialNavigation();
            fixture.whenStable().then(() => {
                component.menuSelect('page2').then(() => {
                    expect(router.url).toEqual('/page2');
                    expect(titleService.getTitle()).toEqual(component.title);
                });
            });
        });
    }));
});


import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // Don't like animations? Replace this with NoopAnimationsModule
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { MatButtonModule, MatSidenavModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ConfigService } from './services/configService';

import {
    RufAppCanvasModule,
    RufBannerModule,
    RufFooterModule,
    RufIconModule,
    RufLayoutModule,
    RufMenubarModule,
    RufPageHeaderModule,
    RufShellModule,
    RufSidemenuModule
} from '@ruf/shell';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './core/core.module';

export function appInitializerFn(config: ConfigService) {
    return () => config.loadAppConfig();
}

@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        FlexLayoutModule,
        MatButtonModule,
        MatSidenavModule,
        BrowserAnimationsModule,
        RufShellModule,
        RufAppCanvasModule,
        RufLayoutModule,
        RufBannerModule,
        RufFooterModule,
        RufIconModule,
        RufMenubarModule,
        RufSidemenuModule,
        RufPageHeaderModule,
        AppRoutingModule,
        CoreModule
    ],
    entryComponents: [],
    providers: [
        Title,
        ConfigService,
        {
            provide: APP_INITIALIZER,
            useFactory: appInitializerFn,
            multi: true,
            deps: [ConfigService]
        }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }

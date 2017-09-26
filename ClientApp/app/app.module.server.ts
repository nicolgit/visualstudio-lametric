import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { HttpModule } from '@angular/http';

import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './components/app/app.component';


@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        ServerModule,
        HttpModule,        
        AppModuleShared
    ]
})
export class AppModule {
}

import { NgModule, ErrorHandler } from "@angular/core"
import { BrowserModule } from "@angular/platform-browser"
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { AppModuleShared } from "./app.module.shared"
import { AppComponent } from "./components/app/app.component"
import { GlobalErrorHandler } from "./core/modules/globalErrorHandler"

@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppModuleShared
    ],
    providers: [
        {
            provide: "BASE_URL",
            useFactory: getBaseUrl
        },
        {
            provide: ErrorHandler,
            useClass: GlobalErrorHandler
        }
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return document.getElementsByTagName("base")[0].href;
}

import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from "@angular/core"
import { CommonModule } from "@angular/common"
import { FormsModule } from "@angular/forms"
import { HttpModule } from "@angular/http"
import { RouterModule } from "@angular/router"

import { AppComponent } from "./components/app/app.component"
import { NavMenuComponent } from "./components/navmenu/navmenu.component"
import { HomeComponent } from "./components/home/home.component"
import { FetchDataComponent } from "./components/fetchdata/fetchdata.component"
import { CounterComponent } from "./components/counter/counter.component"

import { NotificationsService, SimpleNotificationsModule } from "angular2-notifications"
import { LocalStorageService, LocalStorageModule } from "angular-2-local-storage"
import ConfigService from "./core/services/configService"
import SignalRService from "./core/services/signalRService"

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent
    ],
    providers: [
        NotificationsService,
        LocalStorageService,
        ConfigService,
        SignalRService
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        SimpleNotificationsModule,
        LocalStorageModule.withConfig({
            prefix: "web-app-core",
            storageType: "localStorage"
        }),
        RouterModule.forRoot([
            { path: "", redirectTo: "home", pathMatch: "full" },
            { path: "home", component: HomeComponent },
            { path: "counter", component: CounterComponent },
            { path: "fetch-data", component: FetchDataComponent },
            { path: "**", redirectTo: "home" }
        ])
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class AppModuleShared {
}
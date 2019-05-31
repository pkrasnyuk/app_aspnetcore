import { NgModule } from "@angular/core"
import { ServerModule } from "@angular/platform-server"
import { AppModuleShared } from "./app.module.shared"
import { AppComponent } from "./components/app/app.component"
import { LocationStrategy, HashLocationStrategy } from "@angular/common"

import AccountService from "./core/services/accountService"
import AlbumsService from "./core/services/albumsService"
import PhotosService from "./core/services/photosService"
import UsersService from "./core/services/usersService"

@NgModule({
    providers: [
        AccountService,
        AlbumsService,
        PhotosService,
        UsersService,
        {
            provide: LocationStrategy, 
            useClass: HashLocationStrategy
        }
    ],
    bootstrap: [ AppComponent ],
    imports: [
        ServerModule,
        AppModuleShared
    ]
})

export class AppModule {
}
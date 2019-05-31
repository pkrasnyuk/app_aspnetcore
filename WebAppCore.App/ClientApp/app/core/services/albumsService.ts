import { Injectable } from "@angular/core"
import { Http } from "@angular/http"
import { LocalStorageService } from "angular-2-local-storage"

import AlbumModel from "./../domain/albumModel"
import AlbumViewModel from "./../domain/albumViewModel"
import EntitiesService from "./entitiesService"
import ConfigService from "./configService"

@Injectable()
export default class AlbumsService extends EntitiesService<AlbumModel, AlbumViewModel> {

    constructor(public http: Http,
        public configService: ConfigService,
        public localStorageService: LocalStorageService) {

        super(http, configService, localStorageService, "albums");
    }
}
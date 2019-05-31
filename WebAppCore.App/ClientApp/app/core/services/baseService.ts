import { Injectable } from "@angular/core"
import { Http, RequestOptions } from "@angular/http"
import { LocalStorageService } from "angular-2-local-storage"

import ConfigService from "./configService"
import { AuthEx } from "./../modules/commonModule"

@Injectable()
export default class BaseService {
    protected apiUrl: String;
    protected authRequestOptions: RequestOptions;

    constructor(public http: Http,
        public configService: ConfigService,
        public localStorageService: LocalStorageService) {

        configService.loadConfiguration().subscribe(response => {
                if (response && response.api && response.api.url) {
                    this.apiUrl = response.api.url;
                }
            },
            error => console.error(error));

        this.authRequestOptions = AuthEx.createAuthenticationHeader(true, localStorageService);
    }
}
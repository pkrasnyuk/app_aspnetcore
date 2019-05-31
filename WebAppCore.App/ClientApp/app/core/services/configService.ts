import { Injectable, Inject } from "@angular/core"
import { Http, Response } from "@angular/http"
import { Observable } from "rxjs/Rx"

@Injectable()
export default class ConfigService {
    baseUrl: String;

    constructor(public http: Http, @Inject("BASE_URL") baseUrl: String) {
        this.baseUrl = baseUrl;
    }

    loadConfiguration() {
        return this.http.get(`${this.baseUrl}dist/configuration.json`)
            .map((response: Response) => response.json())
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        return Observable.throw(error.json().error || "Internal Server Error");
    }
}
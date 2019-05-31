import { Injectable } from "@angular/core"
import { Http, Response } from "@angular/http"
import { LocalStorageService } from "angular-2-local-storage"
import "rxjs/add/operator/toPromise"

import PageParameters from "./../domain/pageParameters"
import PagedResults from "./../domain/pagedResults"
import BaseService from "./baseService"
import ConfigService from "./configService"

@Injectable()
export default class EntitiesService<TEntity, TEntityViewModel extends object> extends BaseService {

    constructor(public http: Http,
        public configService: ConfigService,
        public localStorageService: LocalStorageService,
        protected readonly entitiesName: String) {

        super(http, configService, localStorageService);
    }

    getEntities() {
        return this.http.get(`${this.apiUrl}/${this.entitiesName}`, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as TEntityViewModel[];
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    getEntitiesWithPagination(parameters: PageParameters) {
        return this.http.post(`${this.apiUrl}/${this.entitiesName}/withPagination`, parameters, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as PagedResults<TEntityViewModel>[];
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    getEntity(id: String) {
        return this.http.get(`${this.apiUrl}/${this.entitiesName}/${id}`, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as TEntityViewModel;
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    createEntity(model: TEntity) {
        return this.http.post(`${this.apiUrl}/${this.entitiesName}`, model, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as TEntityViewModel;
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    updateEntity(id: String, model: TEntity) {
        return this.http.put(`${this.apiUrl}/${this.entitiesName}/${id}`, model, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as TEntityViewModel;
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    deleteEntity(id: String) {
        return this.http.delete(`${this.apiUrl}/${this.entitiesName}/${id}`, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as String;
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }
}
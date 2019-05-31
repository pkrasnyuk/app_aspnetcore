import { Injectable } from "@angular/core"
import { Http, Response } from "@angular/http"
import { LocalStorageService } from "angular-2-local-storage"
import "rxjs/add/operator/toPromise"

import UpdateUserModel from "./../domain/updateUserModel"
import UserViewModel from "./../domain/userViewModel"
import BaseService from "./baseService"
import ConfigService from "./configService"

@Injectable()
export default class UsersService extends BaseService {

    constructor(public http: Http,
        public configService: ConfigService,
        public localStorageService: LocalStorageService) {

        super(http, configService, localStorageService);
    }

    getUsers() {
        return this.http.get(`${this.apiUrl}/users`, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as UserViewModel[];
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    getUser(id: String) {
        return this.http.get(`${this.apiUrl}/users/${id}`, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as UserViewModel;
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    updateUser(id: String, model: UpdateUserModel) {
        return this.http.put(`${this.apiUrl}/users/${id}`, model, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as UserViewModel;
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    deleteUser(id: String) {
        return this.http.delete(`${this.apiUrl}/users/${id}`, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as String;
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }
}
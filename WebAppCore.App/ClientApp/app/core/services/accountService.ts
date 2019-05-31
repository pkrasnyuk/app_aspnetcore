import { Injectable } from "@angular/core"
import { Http, Response } from "@angular/http"
import { LocalStorageService } from "angular-2-local-storage"
import "rxjs/add/operator/toPromise"

import LoginUserModel from "./../domain/loginUserModel"
import RegisterUserModel from "./../domain/registerUserModel"
import UserModel from "./../domain/userModel"
import UserViewModel from "./../domain/userViewModel"
import BaseService from "./baseService"
import ConfigService from "./configService"

@Injectable()
export default class AccountService extends BaseService {

    constructor(public http: Http,
        public configService: ConfigService,
        public localStorageService: LocalStorageService) {

        super(http, configService, localStorageService);
    }

    loginUser(model: LoginUserModel) {
        return this.http.post(`${this.apiUrl}/account/loginUser`, model).toPromise()
            .then((response: Response) => {
                var userModel = response.json() as UserModel;
                if (userModel && userModel.token) {
                    this.localStorageService.set("currentUser", JSON.stringify(userModel));
                }
                return userModel;
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    logoutUser() {
        this.localStorageService.remove("currentUser");
        this.http.post(`${this.apiUrl}/account/logoutUser`, null, this.authRequestOptions).toPromise()
            .then()
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    registerUser(model: RegisterUserModel) {
        return this.http.post(`${this.apiUrl}/account/registerUser`, model).toPromise()
            .then((response: Response) => {
                return response.json() as UserViewModel;
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }
}
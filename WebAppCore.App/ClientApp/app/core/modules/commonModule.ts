import { Headers, RequestOptions } from "@angular/http"
import { LocalStorageService } from "angular-2-local-storage"

import UserModel from "./../domain/userModel"

export class AuthEx {

    static createAuthenticationHeader(withContextType: any, localStorageService: LocalStorageService) {
        const currentUser: UserModel = JSON.parse(String(localStorageService.get("currentUser")));
        if (currentUser && currentUser.token) {
            const headers = withContextType
                ? new Headers({
                    'Content-Type': "application/json;charset=utf-8",
                    "Authorization": `${currentUser.token}`
                })
                : new Headers({
                    "Authorization": `${currentUser.token}`
                });

            return new RequestOptions({ headers: headers });
        } else {
            const headers = withContextType
                ? new Headers({
                    'Content-Type': "application/json;charset=utf-8"
                })
                : new Headers();

            return new RequestOptions({ headers: headers });
        }
    }
}
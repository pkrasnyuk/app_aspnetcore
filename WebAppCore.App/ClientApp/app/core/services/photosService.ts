import { Injectable } from "@angular/core"
import { Http, Response } from "@angular/http"
import { LocalStorageService } from "angular-2-local-storage"
import "rxjs/add/operator/toPromise"

import PhotoModel from "./../domain/photoModel"
import PhotoViewModel from "./../domain/photoViewModel"
import PageParameters from "./../domain/pageParameters"
import PagedResults from "./../domain/pagedResults"
import EntitiesService from "./entitiesService"
import ConfigService from "./configService"

@Injectable()
export default class PhotosService extends EntitiesService<PhotoModel, PhotoViewModel> {

    constructor(public http: Http,
        public configService: ConfigService,
        public localStorageService: LocalStorageService) {

        super(http, configService, localStorageService, "photos");
    }

    getPhotosByAlbumId(albumId: String) {
        return this.http.get(`${this.apiUrl}/${this.entitiesName}/byAlbumId/${albumId}`, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as PhotoViewModel[];
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    getPhotosByAlbumIdWithPagination(albumId: String, parameters: PageParameters) {
        return this.http.post(`${this.apiUrl}/${this.entitiesName}/byAlbumId/${albumId}/withPagination`,
                parameters,
                this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as PagedResults<PhotoViewModel>[];
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }

    uploadPhotoFile(id: String, filePayload: FormData) {
        return this.http.post(`${this.apiUrl}/${this.entitiesName}/${id}/fileUpload`, filePayload, this.authRequestOptions).toPromise()
            .then((response: Response) => {
                return response.json() as PhotoViewModel;
            })
            .catch((error: Response) => {
                return Promise.reject(error.json());
            });
    }
}
import { EventEmitter, Injectable } from "@angular/core";
import { Http } from "@angular/http"
import { HubConnection } from "@aspnet/signalr-client";

import ConfigService from "./configService"
import NotificationModel from "./../domain/notificationModel"

@Injectable()
export default class SignalRService {
    
    notificationReceived = new EventEmitter<NotificationModel>();
    connectionEstablished = new EventEmitter<Boolean>();
    connectionExists = false;

    private hubConnection: HubConnection;

    constructor(public http: Http,
        public configService: ConfigService) {

        configService.loadConfiguration().subscribe(response => {
                if (response && response.signalR && response.signalR.notificationsPublisherUrl) {
                    this.hubConnection = new HubConnection(response.signalR.notificationsPublisherUrl);
                    this.registerOnServerEvents();
                    this.startConnection();
                }
            },
            error => console.error(error));
    }

    public publishNotification(model: NotificationModel) {
        this.hubConnection.invoke("PublishNotification", model);
    }

    private startConnection(): void {
        this.hubConnection.start()
            .then(() => {
                console.log("Hub connection started");
                this.connectionEstablished.emit(true);
            })
            .catch(err => {
                console.log("Error while establishing connection");
            });
    }

    private registerOnServerEvents(): void {
        this.hubConnection.on("OnSendingNotification", (data: any) => {
            this.notificationReceived.emit(data);
        });
    }
}
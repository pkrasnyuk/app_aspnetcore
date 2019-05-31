import { Component, NgZone } from "@angular/core"
import { NotificationsService } from "angular2-notifications"

import SignalRService from "../../core/services/signalRService"
import NotificationModel from "../../core/domain/notificationModel"

@Component({
    selector: "app",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.css"]
})

export class AppComponent {
    notificationOptions: Object;

    constructor(private readonly signalRService: SignalRService,
        private readonly notificationService: NotificationsService,
        private readonly ngZone: NgZone) {

        this.notificationOptions = {
            timeOut: 5000,
            showProgressBar: true,
            pauseOnHover: true
        };

        this.subscribeToEvents();
    }

    private subscribeToEvents(): void {
        this.signalRService.notificationReceived.subscribe((model: NotificationModel) => {
            this.ngZone.run(() => {
                this.notificationService.info("App Notification", model.Message as string);
            });
        });
    }
}

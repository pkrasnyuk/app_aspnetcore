import { ErrorHandler, Injectable } from "@angular/core"
import { NotificationsService } from "angular2-notifications"

@Injectable()
export class GlobalErrorHandler extends ErrorHandler {

    constructor(public notificationService: NotificationsService) {
        super(false);
    }

    handleError(error: any) {
        this.notificationService.error("Application Error", error.messages || "Internal Server Error");
    }
}
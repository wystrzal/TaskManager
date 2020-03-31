import { Component, OnInit } from "@angular/core";
import { Message } from "src/app/models/message.model";
import { MessageService } from "src/app/services/message.service";
import { ErrorService } from "src/app/services/error.service";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: "app-inbox-received",
  templateUrl: "./inbox-received.component.html",
  styleUrls: ["./inbox-received.component.css"]
})
export class InboxReceivedComponent implements OnInit {
  messages: Message[];
  skip = 0;

  constructor(
    private messageService: MessageService,
    private errorService: ErrorService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.getReceivedMessages(this.skip);
  }

  getReceivedMessages(skip: number) {
    this.messageService
      .getReceivedMessages(this.authService.decodedToken.nameid, skip)
      .subscribe(
        message => {
          if (this.messages == null) {
            this.messages = message;
          } else {
            this.messages.push(...message);
          }
        },
        error => {
          this.errorService.newError(error);
        }
      );
  }

  onScroll() {
    this.skip += 10;
    this.getReceivedMessages(this.skip);
  }
}

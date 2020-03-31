import { Component, OnInit, OnChanges } from "@angular/core";
import { Message } from "src/app/models/message.model";
import { MessageService } from "src/app/services/message.service";
import { ErrorService } from "src/app/services/error.service";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: "app-inbox-sended",
  templateUrl: "./inbox-sended.component.html",
  styleUrls: ["./inbox-sended.component.css"]
})
export class InboxSendedComponent implements OnInit {
  messages: Message[];
  skip = 0;

  constructor(
    private messageService: MessageService,
    private errorService: ErrorService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.getSendedMessages(this.skip);
    this.messageService.newMessage.subscribe(() => {
      this.messages = null;
      this.skip = 0;
      this.getSendedMessages(this.skip);
    });
  }

  getSendedMessages(skip: number) {
    this.messageService
      .getSendedMessages(this.authService.decodedToken.nameid, skip)
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
    this.getSendedMessages(this.skip);
  }
}

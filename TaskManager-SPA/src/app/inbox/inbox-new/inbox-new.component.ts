import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { AuthService } from "src/app/services/auth.service";
import { MessageService } from "src/app/services/message.service";
import { ErrorService } from "src/app/services/error.service";

@Component({
  selector: "app-inbox-new",
  templateUrl: "./inbox-new.component.html",
  styleUrls: ["./inbox-new.component.css"],
  encapsulation: ViewEncapsulation.None
})
export class InboxNewComponent implements OnInit {
  model: any = {};
  recipientNick: string;
  sended = false;

  constructor(
    private messageService: MessageService,
    private errorService: ErrorService,
    private authService: AuthService
  ) {}

  ngOnInit() {}

  sendMessage(form: any) {
    this.messageService
      .sendMessage(
        this.authService.decodedToken.nameid,
        this.recipientNick,
        this.model
      )
      .subscribe(
        () => {
          form.resetForm();
        },
        error => {
          this.errorService.newError(error);
        },
        () => {
          this.sended = true;
          this.messageService.newMessage.next();

          setTimeout(() => {
            this.sended = false;
          }, 3000);
        }
      );
  }
}

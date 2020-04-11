import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { MessageService } from "src/app/inbox/message.service";
import { ErrorService } from "src/app/core/helpers/error.service";

@Component({
  selector: "app-inbox-new",
  templateUrl: "./inbox-new.component.html",
  styleUrls: ["./inbox-new.component.css"],
  encapsulation: ViewEncapsulation.None,
})
export class InboxNewComponent implements OnInit {
  model: any = {};
  recipientNick: string;
  sended = false;

  constructor(
    private messageService: MessageService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {}

  sendMessage(form: any) {
    this.messageService.sendMessage(this.recipientNick, this.model).subscribe(
      () => {
        form.resetForm();
      },
      (error) => {
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

import { Component, OnInit } from "@angular/core";
import { ErrorService } from "src/app/services/error.service";
import { MessageService } from "src/app/services/message.service";
import { Message } from "src/app/models/message.model";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: "app-inbox-received-detail",
  templateUrl: "./inbox-received-detail.component.html",
  styleUrls: ["./inbox-received-detail.component.css"]
})
export class InboxReceivedDetailComponent implements OnInit {
  message: Message;

  constructor(
    private errorService: ErrorService,
    private messageService: MessageService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.getMessage();
  }

  getMessage() {
    this.messageService
      .getMessage(this.activatedRoute.snapshot.params.id)
      .subscribe(
        (message: Message) => {
          this.message = message;
        },
        error => {
          this.errorService.newError(error);
        }
      );
  }

  deleteMessage() {
    const userType = "recipient";

    this.messageService
      .deleteMessage(this.activatedRoute.snapshot.params.id, userType)
      .subscribe(
        () => {
          this.router.navigate(["received"], {
            relativeTo: this.activatedRoute.parent
          });
        },
        error => {
          this.errorService.newError(error);
        }
      );
  }
}

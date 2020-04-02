import { Component, OnInit } from "@angular/core";
import { ErrorService } from "src/app/services/error.service";
import { MessageService } from "src/app/services/message.service";
import { AuthService } from "src/app/services/auth.service";
import { ActivatedRoute, Router } from "@angular/router";
import { Message } from "src/app/models/message.model";

@Component({
  selector: "app-inbox-sended-detail",
  templateUrl: "./inbox-sended-detail.component.html",
  styleUrls: ["./inbox-sended-detail.component.css"]
})
export class InboxSendedDetailComponent implements OnInit {
  message: Message;

  constructor(
    private errorService: ErrorService,
    private messageService: MessageService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.getMessage();
  }

  getMessage() {
    this.messageService
      .getMessage(
        this.activatedRoute.snapshot.params.id,
        this.authService.decodedToken.nameid
      )
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
    const userType = "sender";

    this.messageService
      .deleteMessage(
        this.activatedRoute.snapshot.params.id,
        this.authService.decodedToken.nameid,
        userType
      )
      .subscribe(
        () => {
          this.router.navigate(["sended"], {
            relativeTo: this.activatedRoute.parent
          });
        },
        error => {
          this.errorService.newError(error);
        }
      );
  }
}

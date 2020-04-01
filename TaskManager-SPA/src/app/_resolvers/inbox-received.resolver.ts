import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Message } from "@angular/compiler/src/i18n/i18n_ast";
import { MessageService } from "../services/message.service";
import { ErrorService } from "../services/error.service";
import { Observable, of } from "rxjs";
import { AuthService } from "../services/auth.service";
import { catchError } from "rxjs/operators";

@Injectable()
export class InboxReceivedResolver implements Resolve<Message[]> {
  skip = 0;

  constructor(
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router,
    private errorService: ErrorService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
    return this.messageService
      .getReceivedMessages(this.authService.decodedToken.nameid, this.skip)
      .pipe(
        catchError(error => {
          this.errorService.newError("Problem retrieving data");
          this.router.navigate([""]);
          return of(null);
        })
      );
  }
}

import { Injectable } from "@angular/core";
import { CanActivate, Router, ActivatedRouteSnapshot } from "@angular/router";
import { AuthService } from "src/app/shared/services/auth.service";
import { ErrorService } from "../core/helpers/error.service";

@Injectable({
  providedIn: "root",
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private errorService: ErrorService,
    private router: Router
  ) {}

  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }

    this.errorService.newError("Unauthorized.");
    this.router.navigate([""]);
    return false;
  }
}

import { Injectable } from "@angular/core";
import { CanActivate, Router, ActivatedRouteSnapshot } from "@angular/router";
import { AuthService } from "src/app/services/auth.service";
import { ErrorService } from "../core/helpers/error.service";

@Injectable({
  providedIn: "root"
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private errorService: ErrorService,
    private router: Router
  ) {}

  canActivate(next: ActivatedRouteSnapshot): boolean {
    const role = next.firstChild.data.roles as string;
    if (role) {
      const match = this.authService.roleMatch(role);
      if (match) {
        return true;
      } else {
        this.router.navigate([""]);
        this.errorService.newError("Unauthorized.");
      }
    }
    if (this.authService.loggedIn()) {
      return true;
    }

    this.errorService.newError("Unauthorized.");
    this.router.navigate([""]);
    return false;
  }
}

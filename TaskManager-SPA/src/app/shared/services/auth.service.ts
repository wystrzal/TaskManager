import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { map } from "rxjs/operators";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";
import { User } from "../../models/user.model";
import { Subject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  baseUrl = environment.apiUrl + "auth/";
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  user = new Subject<User>();

  constructor(private http: HttpClient, private route: Router) {}

  login(model: any) {
    return this.http.post(this.baseUrl + "login", model).pipe(
      map((response: any) => {
        if (response) {
          localStorage.setItem("token", response.token);
          localStorage.setItem("user", JSON.stringify(response.user));
          this.decodedToken = this.jwtHelper.decodeToken(response.token);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + "register", model);
  }

  logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    this.decodedToken = null;
    this.currentUser = null;
    this.route.navigate([""]);
  }

  loggedIn() {
    const token = localStorage.getItem("token");
    return !this.jwtHelper.isTokenExpired(token);
  }

  roleMatch(allowedRoles: string): boolean {
    const userRoles = this.decodedToken.role.toString();
    if (allowedRoles === userRoles) {
      return true;
    }
    return false;
  }
}

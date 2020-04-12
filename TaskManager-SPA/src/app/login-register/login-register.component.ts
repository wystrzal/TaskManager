import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { AuthService } from "../shared/services/auth.service";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { ErrorService } from "../core/helpers/error.service";

@Component({
  selector: "app-login-register",
  templateUrl: "./login-register.component.html",
  styleUrls: ["./login-register.component.css"],
})
export class LoginRegisterComponent implements OnInit {
  @Output() canceled = new EventEmitter<boolean>();
  loginSelected = true;
  modelLogin: any = {};
  modelRegister: any = {};

  constructor(
    private authService: AuthService,
    private errorService: ErrorService,
    private route: Router
  ) {}

  ngOnInit() {}

  login() {
    this.authService.login(this.modelLogin).subscribe(
      () => {
        this.authService.user.next(JSON.parse(localStorage.getItem("user")));
      },
      (error) => {
        this.errorService.newError(error);
      },
      () => {
        this.route.navigate(["/inbox"]);
      }
    );
  }

  register() {
    this.authService.register(this.modelRegister).subscribe(
      (next) => {
        this.modelLogin.username = this.modelRegister.username;
        this.selectLogin();
      },
      (error) => {
        this.errorService.newError(error);
      }
    );
  }

  cancel() {
    this.canceled.emit(false);
  }

  selectLogin() {
    this.loginSelected = true;
  }

  selectRegister() {
    this.loginSelected = false;
  }
}

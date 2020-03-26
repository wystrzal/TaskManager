import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { ErrorService } from "../services/error.service";

@Component({
  selector: "app-login-register",
  templateUrl: "./login-register.component.html",
  styleUrls: ["./login-register.component.css"]
})
export class LoginRegisterComponent implements OnInit {
  @Output() canceled = new EventEmitter<boolean>();
  loginSelected = true;
  model: any = {};

  constructor(
    private authService: AuthService,
    private errorService: ErrorService,
    private route: Router
  ) {}

  ngOnInit() {}

  login() {
    this.authService.login(this.model).subscribe(
      next => {
        this.route.navigate(["statistics"]);
      },
      error => {
        this.errorService.newError(error);
      }
    );
  }

  cancel() {
    this.canceled.emit(false);
  }

  selectLogin() {
    this.loginSelected = true;
    this.model = {};
  }

  selectRegister() {
    this.loginSelected = false;
    this.model = {};
  }
}

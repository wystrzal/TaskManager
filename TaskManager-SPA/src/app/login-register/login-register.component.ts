import { Component, OnInit, Output, EventEmitter } from "@angular/core";

@Component({
  selector: "app-login-register",
  templateUrl: "./login-register.component.html",
  styleUrls: ["./login-register.component.css"]
})
export class LoginRegisterComponent implements OnInit {
  @Output() canceled = new EventEmitter<boolean>();
  login: boolean;

  constructor() {}

  ngOnInit() {
    this.login = true;
  }

  cancel() {
    this.canceled.emit(false);
  }
}

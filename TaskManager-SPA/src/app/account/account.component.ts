import { Component, OnInit } from "@angular/core";
import { UserService } from "./user.service";
import { AuthService } from "../shared/services/auth.service";
import { User } from "../models/user.model";
import { ErrorService } from "../core/helpers/error.service";

@Component({
  selector: "app-account",
  templateUrl: "./account.component.html",
  styleUrls: ["./account.component.css"],
})
export class AccountComponent implements OnInit {
  userPhotos: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9];
  selectPhoto = false;
  user: User = { nickname: "", photoId: 0 };
  nickModel: any = {};
  passwordModel: any = {};

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {
    this.user.photoId = 1;
    this.getUser();
  }

  getUser() {
    this.userService.getUser().subscribe(
      (user: User) => {
        this.user = user;
      },
      (error) => {
        this.errorService.newError(error);
      }
    );
  }

  openPhotoChooser() {
    this.selectPhoto = !this.selectPhoto;
  }

  changePhoto(photoId: number) {
    this.userService.changePhoto(photoId).subscribe(
      () => {
        this.user.photoId = photoId;
        this.selectPhoto = false;
      },
      (error) => {
        this.errorService.newError(error);
      }
    );
  }

  changeNick(form: any) {
    this.userService.changeNick(this.nickModel).subscribe(
      (user: any) => {
        this.user.nickname = this.nickModel.nickname;
        form.reset();
        this.authService.user.next(this.user);
        localStorage.removeItem("user");
        localStorage.setItem("user", JSON.stringify(user.user));
      },
      (error) => {
        this.errorService.newError(error);
      }
    );
  }

  changePassword() {
    this.userService.changePassword(this.passwordModel).subscribe(
      () => {
        this.authService.logout();
      },
      (error) => {
        this.errorService.newError(error);
      }
    );
  }
}

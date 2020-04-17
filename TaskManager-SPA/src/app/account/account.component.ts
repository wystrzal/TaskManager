import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-account",
  templateUrl: "./account.component.html",
  styleUrls: ["./account.component.css"],
})
export class AccountComponent implements OnInit {
  userPhotos: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9];
  selectPhoto = false;

  constructor() {}

  ngOnInit() {}

  changePhoto() {
    if (this.selectPhoto === false) {
      this.selectPhoto = true;
    }
  }
}

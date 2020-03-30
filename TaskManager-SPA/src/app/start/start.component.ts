import { Component, OnInit } from "@angular/core";
import { trigger, transition, style, animate } from "@angular/animations";
import { AuthService } from "../services/auth.service";
import { User } from "../models/user.model";

import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: "app-start",
  templateUrl: "./start.component.html",
  styleUrls: ["./start.component.css"],
  animations: [
    trigger("animationMenu", [
      transition(":enter", [
        style({ width: "100%", opacity: 0 }),
        animate("500ms ease-out", style({ width: "25%", opacity: 1 }))
      ]),
      transition(":leave", [
        style({ width: "25%", opacity: 1 }),
        animate("500ms ease-in", style({ width: "0%", opacity: 0 }))
      ])
    ])
  ]
})
export class StartComponent implements OnInit {
  listsExpanded = false;
  menuOpen = false;
  user: User;

  constructor(private authService: AuthService) {}

  openMenu() {
    this.menuOpen = !this.menuOpen;
  }

  logout() {
    this.authService.logout();
  }

  ngOnInit() {
    this.authService.user.subscribe(user => {
      this.user = user;
    });
    this.user = this.authService.currentUser;
  }
}

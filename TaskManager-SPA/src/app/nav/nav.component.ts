import { Component, OnInit } from "@angular/core";
import { trigger, transition, style, animate } from "@angular/animations";

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.css"],
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
export class NavComponent implements OnInit {
  listsExpanded = false;
  menuOpen = false;

  constructor() {}

  openMenu() {
    this.menuOpen = !this.menuOpen;
  }

  ngOnInit() {}
}

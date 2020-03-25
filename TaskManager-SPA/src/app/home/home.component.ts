import { Component, OnInit } from "@angular/core";
import { trigger, style, transition, animate } from "@angular/animations";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
  animations: [
    trigger("animationPanel", [
      transition(":enter", [
        style({ opacity: 0 }),
        animate("1s ease-out", style({ opacity: 1 }))
      ]),
      transition(":leave", [
        style({ opacity: 1 }),
        animate("1s ease-in", style({ opacity: 0 }))
      ])
    ])
  ]
})
export class HomeComponent implements OnInit {
  displayHome = true;
  start = false;

  constructor() {}

  ngOnInit() {}

  onCancel(event: boolean) {
    this.start = event;
    setTimeout(() => {
      this.displayHome = true;
    }, 1000);
  }

  onStart() {
    this.displayHome = false;
    this.start = true;
  }
}

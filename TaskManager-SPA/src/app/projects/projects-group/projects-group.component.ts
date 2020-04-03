import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-projects-group",
  templateUrl: "./projects-group.component.html",
  styleUrls: ["./projects-group.component.css"]
})
export class ProjectsGroupComponent implements OnInit {
  skip = 0;
  constructor() {}

  ngOnInit() {}

  onScroll() {
    this.skip += 10;
  }
}

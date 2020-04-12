import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-tasks-over-thirty",
  templateUrl: "./tasks-over-thirty.component.html",
  styleUrls: ["./tasks-over-thirty.component.css"],
})
export class TasksOverThirtyComponent implements OnInit {
  priorityOpen: boolean[] = [];
  statusOpen: boolean[] = [];

  constructor() {}

  ngOnInit() {}

  changePriority(id: number) {
    if (!this.priorityOpen[id]) {
      this.priorityOpen[id] = true;
    } else {
      this.priorityOpen[id] = false;
    }
  }

  changeStatus(id: number) {
    if (!this.statusOpen[id]) {
      this.statusOpen[id] = true;
    } else {
      this.statusOpen[id] = false;
    }
  }
}

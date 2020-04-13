import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { BsModalRef } from "ngx-bootstrap";
import { ErrorService } from "src/app/core/helpers/error.service";
import { Task } from "src/app/models/task.model";
import { TaskService } from "../task.service";

@Component({
  selector: "app-projects-tasks-add",
  templateUrl: "./projects-tasks-add.component.html",
  styleUrls: ["./projects-tasks-add.component.css"],
})
export class ProjectsTasksAddComponent implements OnInit {
  @Output() newTask = new EventEmitter<Task>();
  model: any = {};
  userId: number;
  projectId: number;

  constructor(
    public bsModalRef: BsModalRef,
    private taskService: TaskService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {}

  addTask() {
    this.taskService.addTask(this.userId, this.projectId, this.model).subscribe(
      (task: Task) => {
        this.newTask.emit(task);
      },
      (error) => {
        this.errorService.newError(error);
      },
      () => {
        this.bsModalRef.hide();
      }
    );
  }
}

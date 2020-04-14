import { Component, OnInit } from "@angular/core";
import { Location } from "@angular/common";
import { BsModalRef, BsModalService } from "ngx-bootstrap";
import { ProjectsTasksAddComponent } from "./projects-tasks-add/projects-tasks-add.component";
import { Task } from "src/app/models/task.model";
import { AuthService } from "src/app/shared/services/auth.service";
import { ActivatedRoute } from "@angular/router";
import { TaskService } from "./task.service";
import { ErrorService } from "src/app/core/helpers/error.service";

@Component({
  selector: "app-projects-tasks",
  templateUrl: "./projects-tasks.component.html",
  styleUrls: ["./projects-tasks.component.css"],
})
export class ProjectsTasksComponent implements OnInit {
  priorityOpen: boolean[] = [];
  statusOpen: boolean[] = [];
  bsModalRef: BsModalRef;
  tasks: Task[];

  constructor(
    private location: Location,
    private modalService: BsModalService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private taskService: TaskService,
    private errorService: ErrorService
  ) {}

  openTaskAddModal() {
    const initialState = {
      userId: this.authService.decodedToken.nameid,
      projectId: this.activatedRoute.snapshot.params.id,
    };
    this.bsModalRef = this.modalService.show(ProjectsTasksAddComponent, {
      initialState,
    });
    this.bsModalRef.content.newTask.subscribe((task: Task) => {
      this.tasks.push(task);
    });
  }

  ngOnInit() {
    this.getTasks();
  }

  backClicked() {
    this.location.back();
  }

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

  getTasks() {
    this.taskService
      .getTasks(
        this.authService.decodedToken.nameid,
        this.activatedRoute.snapshot.params.id
      )
      .subscribe(
        (tasks) => {
          this.tasks = tasks;
        },
        (error) => {
          this.errorService.newError(error);
        }
      );
  }
}

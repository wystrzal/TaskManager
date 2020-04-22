import { Component, OnInit } from "@angular/core";
import { Location } from "@angular/common";
import { BsModalRef, BsModalService } from "ngx-bootstrap";
import { ProjectsTasksAddComponent } from "./projects-tasks-add/projects-tasks-add.component";
import { Task } from "src/app/models/task.model";
import { AuthService } from "src/app/shared/services/auth.service";
import { ActivatedRoute } from "@angular/router";
import { TaskService } from "../../shared/services/task.service";
import { ErrorService } from "src/app/core/helpers/error.service";
import { ProjectService } from "../project.service";
import { Project } from "src/app/models/project.model";

@Component({
  selector: "app-projects-tasks",
  templateUrl: "./projects-tasks.component.html",
  styleUrls: ["./projects-tasks.component.css"],
})
export class ProjectsTasksComponent implements OnInit {
  currentUser: number;
  priorityOpen: boolean[] = [];
  statusOpen: boolean[] = [];
  bsModalRef: BsModalRef;
  tasks: Task[];
  project: Project;
  priority: string;
  status: string;
  skip = 0;

  constructor(
    private location: Location,
    private modalService: BsModalService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private taskService: TaskService,
    private errorService: ErrorService,
    private projectService: ProjectService
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
    this.getProjectInfo();
  }

  backClicked() {
    this.location.back();
  }

  changePriority(id: number, taskIndex: number) {
    if (!this.priorityOpen[id]) {
      this.priorityOpen[id] = true;
    } else {
      if (this.tasks[taskIndex].priority === this.priority) {
        this.priorityOpen[id] = false;
      } else {
        this.taskService
          .changeStatusPriority(
            this.authService.decodedToken.nameid,
            id,
            "priority",
            this.priority
          )
          .subscribe(
            () => {
              this.tasks[taskIndex].priority = this.priority;
            },
            (error) => {
              this.errorService.newError(error);
            }
          );
        this.priorityOpen[id] = false;
      }
    }
  }

  changeStatus(id: number, taskIndex: number) {
    if (!this.statusOpen[id]) {
      this.statusOpen[id] = true;
    } else {
      if (this.tasks[taskIndex].status === this.status) {
        this.statusOpen[id] = false;
      } else {
        this.taskService
          .changeStatusPriority(
            this.authService.decodedToken.nameid,
            id,
            "status",
            this.status
          )
          .subscribe(
            () => {
              this.tasks[taskIndex].status = this.status;
            },
            (error) => {
              this.errorService.newError(error);
            }
          );
        this.statusOpen[id] = false;
      }
    }
  }

  getTasks() {
    this.taskService
      .getTasks(
        this.authService.decodedToken.nameid,
        this.activatedRoute.snapshot.params.id,
        this.skip
      )
      .subscribe(
        (task) => {
          if (this.tasks == null) {
            this.tasks = task;
          } else {
            this.tasks.push(...task);
          }
          this.currentUser = this.authService.decodedToken.nameid;
        },
        (error) => {
          this.errorService.newError(error);
        }
      );
  }

  getProjectInfo() {
    this.projectService
      .getProject(this.activatedRoute.snapshot.params.id)
      .subscribe(
        (project) => {
          this.project = project;
        },
        (error) => {
          this.errorService.newError(error);
        }
      );
  }

  onScroll() {
    this.skip += 15;
    this.getTasks();
  }

  deleteTask(id: number, taskIndex: number) {
    this.errorService.confirm("Are you sure you want delete?", () => {
      this.taskService
        .deleteTask(this.authService.decodedToken.nameid, id)
        .subscribe(
          () => {
            this.tasks.splice(taskIndex, 1);
          },
          (error) => {
            this.errorService.newError(error);
          }
        );
    });
  }
}

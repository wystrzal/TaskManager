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
import { TooltipPosition } from "@angular/material/tooltip";
import { User } from "src/app/models/user.model";

@Component({
  selector: "app-projects-tasks",
  templateUrl: "./projects-tasks.component.html",
  styleUrls: ["./projects-tasks.component.css"],
})
export class ProjectsTasksComponent implements OnInit {
  position: TooltipPosition = "right";
  currentUser: number;
  priorityOpen: boolean[] = [];
  priority: string;
  statusOpen: boolean[] = [];
  status: string;
  bsModalRef: BsModalRef;
  tasks: Task[];
  project: Project;
  skip = 0;
  projectUsers: User[];

  constructor(
    private location: Location,
    private modalService: BsModalService,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
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

  changePriority(taskId: number, taskIndex: number) {
    if (!this.priorityOpen[taskId]) {
      this.priorityOpen[taskId] = true;
    } else {
      if (this.tasks[taskIndex].priority === this.priority) {
        this.priorityOpen[taskId] = false;
      } else {
        this.taskService
          .changeStatusPriority(taskId, "priority", this.priority)
          .subscribe(
            () => {
              this.tasks[taskIndex].priority = this.priority;
            },
            (error) => {
              this.errorService.newError(error);
            }
          );
        this.priorityOpen[taskId] = false;
      }
    }
  }

  changeStatus(taskId: number, taskIndex: number) {
    if (!this.statusOpen[taskId]) {
      this.statusOpen[taskId] = true;
    } else {
      if (this.tasks[taskIndex].status === this.status) {
        this.statusOpen[taskId] = false;
      } else {
        this.taskService
          .changeStatusPriority(taskId, "status", this.status)
          .subscribe(
            () => {
              this.tasks[taskIndex].status = this.status;
            },
            (error) => {
              this.errorService.newError(error);
            }
          );
        this.statusOpen[taskId] = false;
      }
    }
  }

  changeTaskOwner(taskId: number, taskIndex: number, newOwner: string) {
    this.taskService.changeTaskOwner(taskId, newOwner).subscribe(
      (task: Task) => {
        this.tasks[taskIndex].taskOwnerPhoto = task.taskOwnerPhoto;
        this.tasks[taskIndex].taskOwnerNick = task.taskOwnerNick;
        this.tasks[taskIndex].taskOwner = task.taskOwner;
      },
      (error) => {
        this.errorService.newError(error);
      }
    );
  }

  getTasks() {
    this.taskService
      .getTasks(this.activatedRoute.snapshot.params.id, this.skip)
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

          this.projectService.getProjectUsers(this.project.projectId).subscribe(
            (user) => {
              this.projectUsers = user;
            },
            (error) => {
              this.errorService.newError(error);
            }
          );
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
      this.taskService.deleteTask(id).subscribe(
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

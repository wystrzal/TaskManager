import { Component, OnInit } from "@angular/core";
import { Task } from "../models/task.model";
import { AuthService } from "../shared/services/auth.service";
import { TaskService } from "../shared/services/task.service";
import { ErrorService } from "../core/helpers/error.service";

@Component({
  selector: "app-important",
  templateUrl: "./important.component.html",
  styleUrls: ["./important.component.css"],
})
export class ImportantComponent implements OnInit {
  currentUser: number;
  statusOpen: boolean[] = [];
  importantTasks: Task[];
  status: string;
  skip = 0;

  constructor(
    private authService: AuthService,
    private taskService: TaskService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {
    this.getImportantTasks();
  }

  changeStatus(id: number, taskIndex: number) {
    if (!this.statusOpen[id]) {
      this.statusOpen[id] = true;
    } else {
      if (this.importantTasks[taskIndex].status === this.status) {
        this.statusOpen[id] = false;
      } else {
        this.taskService
          .changeStatusPriority(id, "status", this.status)
          .subscribe(
            () => {
              this.importantTasks[taskIndex].status = this.status;
            },
            (error) => {
              this.errorService.newError(error);
            }
          );
        this.statusOpen[id] = false;
      }
    }
  }

  getImportantTasks() {
    this.taskService.getImportantTasks(this.skip).subscribe(
      (task) => {
        if (this.importantTasks == null) {
          this.importantTasks = task;
        } else {
          this.importantTasks.push(...task);
        }
        this.currentUser = this.authService.decodedToken.nameid;
      },
      (error) => {
        this.errorService.newError(error);
      }
    );
  }

  onScroll() {
    this.skip += 15;
    this.getImportantTasks();
  }
}

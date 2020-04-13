import { Component, OnInit } from "@angular/core";
import { Location } from "@angular/common";
import { BsModalRef, BsModalService } from "ngx-bootstrap";
import { ProjectsTasksAddComponent } from "./projects-tasks-add/projects-tasks-add.component";
import { Task } from "src/app/models/task.model";
import { AuthService } from "src/app/shared/services/auth.service";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-projects-tasks",
  templateUrl: "./projects-tasks.component.html",
  styleUrls: ["./projects-tasks.component.css"],
})
export class ProjectsTasksComponent implements OnInit {
  tasks: Task[];
  bsModalRef: BsModalRef;

  constructor(
    private location: Location,
    private modalService: BsModalService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute
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

  ngOnInit() {}

  backClicked() {
    this.location.back();
  }
}

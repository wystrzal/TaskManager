import { Component, OnInit } from "@angular/core";
import { Project } from "src/app/models/project.model";
import { ProjectService } from "../../project.service";
import { ErrorService } from "src/app/core/helpers/error.service";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-projects-manage-detail",
  templateUrl: "./projects-manage-detail.component.html",
  styleUrls: ["./projects-manage-detail.component.css"],
})
export class ProjectsManageDetailComponent implements OnInit {
  project: Project;

  constructor(
    private projectService: ProjectService,
    private errorService: ErrorService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.getProject();
  }

  getProject() {
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
}

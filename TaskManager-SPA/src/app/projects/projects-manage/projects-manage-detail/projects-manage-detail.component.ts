import { Component, OnInit } from "@angular/core";
import { Project } from "src/app/models/project.model";
import { ProjectService } from "../../project.service";
import { ErrorService } from "src/app/core/helpers/error.service";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: "app-projects-manage-detail",
  templateUrl: "./projects-manage-detail.component.html",
  styleUrls: ["./projects-manage-detail.component.css"],
})
export class ProjectsManageDetailComponent implements OnInit {
  model: any = {};
  project: Project;

  constructor(
    private projectService: ProjectService,
    private errorService: ErrorService,
    private activatedRoute: ActivatedRoute,
    private router: Router
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

  deleteProject() {
    this.projectService
      .deleteProject(this.activatedRoute.snapshot.params.id)
      .subscribe(
        () => {
          this.router.navigate(["../"], { relativeTo: this.activatedRoute });
        },
        (error) => {
          this.errorService.newError(error);
        }
      );
  }
}

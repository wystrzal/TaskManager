import { Component, OnInit } from "@angular/core";
import { Project } from "src/app/models/project.model";
import { ProjectService } from "../../project.service";
import { ErrorService } from "src/app/core/helpers/error.service";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: "app-projects-manage-detail",
  templateUrl: "./projects-manage-detail.component.html",
  styleUrls: ["./projects-manage-detail.component.css"],
})
export class ProjectsManageDetailComponent implements OnInit {
  model: any = {};
  project: Project;
  userId: number;

  constructor(
    private projectService: ProjectService,
    private errorService: ErrorService,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
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
          this.userId = this.authService.decodedToken.nameid;
        },
        (error) => {
          this.errorService.newError(error);
        }
      );
  }

  deleteProject() {
    this.errorService.confirm("Are you sure you want delete?", () => {
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
    });
  }

  leaveProject() {
    this.errorService.confirm("Are you sure you want leave?", () => {
      this.projectService
        .leaveProject(this.activatedRoute.snapshot.params.id)
        .subscribe(
          () => {
            this.router.navigate(["../"], { relativeTo: this.activatedRoute });
          },
          (error) => {
            this.errorService.newError(error);
          }
        );
    });
  }
}

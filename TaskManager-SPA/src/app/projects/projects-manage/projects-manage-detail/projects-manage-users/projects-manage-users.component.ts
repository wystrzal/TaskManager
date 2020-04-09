import { Component, OnInit, Input } from "@angular/core";
import { Project } from "src/app/models/project.model";
import { ProjectService } from "src/app/projects/project.service";
import { ErrorService } from "src/app/core/helpers/error.service";

@Component({
  selector: "app-projects-manage-users",
  templateUrl: "./projects-manage-users.component.html",
  styleUrls: ["./projects-manage-users.component.css"],
})
export class ProjectsManageUsersComponent implements OnInit {
  @Input() project: Project;
  model: any = {};

  constructor(
    private projectService: ProjectService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {}

  addToProject(form: any) {
    this.projectService
      .addToProject(this.project.projectId, this.model.userNick)
      .subscribe(
        () => {
          form.reset();
        },
        (error) => {
          this.errorService.newError(error);
        }
      );
  }
}

import { Component, OnInit } from "@angular/core";
import { Project } from "src/app/models/project.model";
import { ProjectService } from "../project.service";
import { ErrorService } from "src/app/core/helpers/error.service";

@Component({
  selector: "app-projects-group",
  templateUrl: "./projects-group.component.html",
  styleUrls: ["./projects-group.component.css"],
})
export class ProjectsGroupComponent implements OnInit {
  projects: Project[];
  skip = 0;

  constructor(
    private projectService: ProjectService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {
    this.getProjects();
  }

  onScroll() {
    this.skip += 10;
    this.getProjects();
  }

  onAcceptInvite(project: Project) {
    this.projects.push(project);
  }

  getProjects() {
    this.projectService.getProjects("group", this.skip).subscribe(
      (project) => {
        if (this.projects == null) {
          this.projects = project;
        } else {
          this.projects.push(...project);
        }
      },
      (error) => {
        this.errorService.newError(error);
      }
    );
  }
}

import { Component, OnInit } from "@angular/core";
import { ProjectService } from "../project.service";
import { Project } from "src/app/models/project.model";
import { ErrorService } from "src/app/core/helpers/error.service";

@Component({
  selector: "app-projects-personal",
  templateUrl: "./projects-personal.component.html",
  styleUrls: ["./projects-personal.component.css"],
})
export class ProjectsPersonalComponent implements OnInit {
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
    this.skip += 15;
    this.getProjects();
  }

  getProjects() {
    this.projectService.getProjects("personal", this.skip).subscribe(
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

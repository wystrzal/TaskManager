import { Component, OnInit } from "@angular/core";
import { BsModalRef, BsModalService } from "ngx-bootstrap";
import { ProjectsManageAddComponent } from "./projects-manage-add/projects-manage-add.component";
import { ProjectService } from "../project.service";
import { Project } from "src/app/models/project.model";
import { ErrorService } from "src/app/core/helpers/error.service";

@Component({
  selector: "app-projects-manage",
  templateUrl: "./projects-manage.component.html",
  styleUrls: ["./projects-manage.component.css"],
})
export class ProjectsManageComponent implements OnInit {
  projects: Project[];
  bsModalRef: BsModalRef;
  skip = 0;

  constructor(
    private modalService: BsModalService,
    private projectService: ProjectService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {
    this.getProjects();
  }

  openProjectAddModal() {
    this.bsModalRef = this.modalService.show(ProjectsManageAddComponent);
    this.bsModalRef.content.newProject.subscribe((project: Project) => {
      this.projects.push(project);
    });
  }

  onScroll() {
    this.skip += 10;
    this.getProjects();
  }

  getProjects() {
    this.projectService.getProjects("all", this.skip).subscribe(
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

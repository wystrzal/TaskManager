import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { BsModalRef } from "ngx-bootstrap";
import { ProjectService } from "../../project.service";
import { ErrorService } from "src/app/core/helpers/error.service";
import { Project } from "src/app/models/project.model";

@Component({
  selector: "app-projects-manage-add",
  templateUrl: "./projects-manage-add.component.html",
  styleUrls: ["./projects-manage-add.component.css"],
})
export class ProjectsManageAddComponent implements OnInit {
  @Output() newProject = new EventEmitter<Project>();
  model: any = {};

  constructor(
    public bsModalRef: BsModalRef,
    private projectService: ProjectService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {
    this.model.type = "personal";
  }

  addProject() {
    this.projectService.addProject(this.model).subscribe(
      (project: Project) => {
        this.newProject.emit(project);
      },
      (error) => {
        this.errorService.newError(error);
      },
      () => {
        this.bsModalRef.hide();
      }
    );
  }
}

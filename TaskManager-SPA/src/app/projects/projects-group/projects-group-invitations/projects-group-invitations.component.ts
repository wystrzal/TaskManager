import { Component, OnInit } from "@angular/core";
import { ProjectService } from "../../project.service";
import { Project } from "src/app/models/project.model";
import { ErrorService } from "src/app/core/helpers/error.service";

@Component({
  selector: "app-projects-group-invitations",
  templateUrl: "./projects-group-invitations.component.html",
  styleUrls: ["./projects-group-invitations.component.css"],
})
export class ProjectsGroupInvitationsComponent implements OnInit {
  invitations: Project[];

  constructor(
    private projectService: ProjectService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {
    this.getInvitations();
  }

  getInvitations() {
    this.projectService.getInvitations().subscribe(
      (invitations) => {
        this.invitations = invitations;
      },
      (error) => {
        this.errorService.newError(error);
      }
    );
  }
}

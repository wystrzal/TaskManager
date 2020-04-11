import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { ProjectService } from "../../project.service";
import { Project } from "src/app/models/project.model";
import { ErrorService } from "src/app/core/helpers/error.service";

@Component({
  selector: "app-projects-group-invitations",
  templateUrl: "./projects-group-invitations.component.html",
  styleUrls: ["./projects-group-invitations.component.css"],
})
export class ProjectsGroupInvitationsComponent implements OnInit {
  @Output() project = new EventEmitter<Project>();
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

  joinToProject(projectId: number, action: number, invitationIndex: number) {
    this.projectService.joinToProject(projectId, action).subscribe(
      (project: Project) => {
        this.invitations.splice(invitationIndex, 1);
        if (project) {
          this.project.next(project);
        }
      },
      (error) => {
        this.errorService.newError(error);
      }
    );
  }
}

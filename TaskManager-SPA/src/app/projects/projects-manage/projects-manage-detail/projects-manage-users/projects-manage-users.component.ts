import { Component, OnInit, Input } from "@angular/core";
import { Project } from "src/app/models/project.model";
import { ProjectService } from "src/app/projects/project.service";
import { ErrorService } from "src/app/core/helpers/error.service";
import { ActivatedRoute } from "@angular/router";
import { User } from "src/app/models/user.model";

@Component({
  selector: "app-projects-manage-users",
  templateUrl: "./projects-manage-users.component.html",
  styleUrls: ["./projects-manage-users.component.css"],
})
export class ProjectsManageUsersComponent implements OnInit {
  @Input() project: Project;
  @Input() userId: number;
  model: any = {};
  sended = false;
  users: User[];

  constructor(
    private projectService: ProjectService,
    private errorService: ErrorService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.getProjectUsers();
  }

  getProjectUsers() {
    this.projectService
      .getProjectUsers(this.activatedRoute.snapshot.params.id)
      .subscribe(
        (user) => {
          this.users = user;
        },
        (error) => {
          this.errorService.newError(error);
        }
      );
  }

  addToProject(form: any) {
    this.projectService
      .addToProject(this.project.projectId, this.model.userNick)
      .subscribe(
        () => {
          form.reset();
        },
        (error) => {
          this.errorService.newError(error);
        },
        () => {
          this.sended = true;

          setTimeout(() => {
            this.sended = false;
          }, 3000);
        }
      );
  }

  deleteFromProject(userToDelete: number, userIndex: number) {
    this.projectService
      .deleteFromProject(this.project.projectId, userToDelete)
      .subscribe(
        () => {
          this.users.splice(userIndex, 1);
        },
        (error) => {
          this.errorService.newError(error);
        }
      );
  }
}

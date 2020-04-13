import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { ProjectsComponent } from "./projects.component";
import { ProjectsRoutes } from "./projects.routing";
import { ProjectsGroupComponent } from "./projects-group/projects-group.component";
import { ProjectsPersonalComponent } from "./projects-personal/projects-personal.component";
import { ProjectsManageComponent } from "./projects-manage/projects-manage.component";
import { ProjectsManageAddComponent } from "./projects-manage/projects-manage-add/projects-manage-add.component";
import { ProjectsManageDetailComponent } from "./projects-manage/projects-manage-detail/projects-manage-detail.component";
import { ProjectsGroupInvitationsComponent } from "./projects-group/projects-group-invitations/projects-group-invitations.component";
import { ProjectsManageUsersComponent } from "./projects-manage/projects-manage-detail/projects-manage-users/projects-manage-users.component";
import { ProjectsTasksComponent } from "./projects-tasks/projects-tasks.component";
import { TasksWithinSevenComponent } from "./projects-tasks/tasks-within-seven/tasks-within-seven.component";
import { TasksWithinThirtyComponent } from "./projects-tasks/tasks-within-thirty/tasks-within-thirty.component";
import { TasksOverThirtyComponent } from "./projects-tasks/tasks-over-thirty/tasks-over-thirty.component";
import { ProjectsTasksAddComponent } from "./projects-tasks/projects-tasks-add/projects-tasks-add.component";

@NgModule({
  declarations: [
    ProjectsComponent,
    ProjectsGroupComponent,
    ProjectsPersonalComponent,
    ProjectsManageComponent,
    ProjectsManageAddComponent,
    ProjectsManageDetailComponent,
    ProjectsGroupInvitationsComponent,
    ProjectsManageUsersComponent,
    ProjectsTasksComponent,
    TasksWithinSevenComponent,
    TasksWithinThirtyComponent,
    TasksOverThirtyComponent,
    ProjectsTasksAddComponent,
  ],
  imports: [SharedModule, ProjectsRoutes],
  entryComponents: [ProjectsManageAddComponent, ProjectsTasksAddComponent],
})
export class ProjectsModule {}

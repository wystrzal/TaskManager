import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { ProjectsComponent } from "./projects.component";
import { ProjectsRoutes } from "./projects.routing";
import { ProjectsGroupComponent } from "./projects-group/projects-group.component";
import { ProjectsPersonalComponent } from "./projects-personal/projects-personal.component";
import { ProjectsManageComponent } from "./projects-manage/projects-manage.component";
import { ProjectsManageAddComponent } from "./projects-manage/projects-manage-add/projects-manage-add.component";

@NgModule({
  declarations: [
    ProjectsComponent,
    ProjectsGroupComponent,
    ProjectsPersonalComponent,
    ProjectsManageComponent,
    ProjectsManageAddComponent
  ],
  imports: [SharedModule, ProjectsRoutes],
  entryComponents: [ProjectsManageAddComponent]
})
export class ProjectsModule {}

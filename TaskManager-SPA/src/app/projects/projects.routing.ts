import { Routes, RouterModule } from "@angular/router";
import { ProjectsComponent } from "./projects.component";
import { ProjectsGroupComponent } from "./projects-group/projects-group.component";
import { ProjectsPersonalComponent } from "./projects-personal/projects-personal.component";
import { ProjectsManageComponent } from "./projects-manage/projects-manage.component";
import { ProjectsManageDetailComponent } from "./projects-manage/projects-manage-detail/projects-manage-detail.component";

const routes: Routes = [
  {
    path: "",
    component: ProjectsComponent,
    children: [
      {
        path: "group",
        component: ProjectsGroupComponent,
      },
      {
        path: "personal",
        component: ProjectsPersonalComponent,
      },
      {
        path: "manage",
        component: ProjectsManageComponent,
      },
      {
        path: "manage/:id",
        component: ProjectsManageDetailComponent,
      },
    ],
  },
];

export const ProjectsRoutes = RouterModule.forChild(routes);

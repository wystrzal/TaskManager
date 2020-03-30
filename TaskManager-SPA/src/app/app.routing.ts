import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { AuthGuard } from "./guards/auth.guard";
import { InboxComponent } from "./inbox/inbox.component";
import { ProjectsComponent } from "./projects/projects.component";

export const AppRoutes: Routes = [
  { path: "", component: HomeComponent },
  {
    path: "",
    runGuardsAndResolvers: "always",
    canActivate: [AuthGuard],
    children: [
      { path: "inbox", component: InboxComponent },
      { path: "projects", component: ProjectsComponent }
    ]
  },
  { path: "**", redirectTo: "", pathMatch: "full" }
];

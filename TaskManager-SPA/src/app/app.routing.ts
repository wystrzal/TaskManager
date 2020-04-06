import { Routes } from "@angular/router";
import { AuthGuard } from "./guards/auth.guard";
import { StartComponent } from "./start/start.component";

export const AppRoutes: Routes = [
  { path: "", component: StartComponent },
  {
    path: "",
    runGuardsAndResolvers: "always",
    canActivate: [AuthGuard],
    children: [
      {
        path: "inbox",
        loadChildren: "./inbox/inbox.module#InboxModule",
      },
      {
        path: "projects",
        loadChildren: "./projects/projects.module#ProjectsModule",
      },
    ],
  },
  { path: "**", redirectTo: "", pathMatch: "full" },
];

import { Routes } from "@angular/router";
import { AuthGuard } from "./guards/auth.guard";
import { StartComponent } from "./start/start.component";
import { ImportantComponent } from "./important/important.component";
import { AccountComponent } from "./account/account.component";

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
      { path: "important", component: ImportantComponent },
      { path: "account", component: AccountComponent },
    ],
  },
  { path: "**", redirectTo: "", pathMatch: "full" },
];

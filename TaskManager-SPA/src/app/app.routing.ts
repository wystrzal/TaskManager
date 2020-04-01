import { Routes } from "@angular/router";
import { AuthGuard } from "./guards/auth.guard";
import { InboxComponent } from "./inbox/inbox.component";
import { ProjectsComponent } from "./projects/projects.component";
import { StartComponent } from "./start/start.component";
import { InboxReceivedDetailComponent } from "./inbox/inbox-received/inbox-received-detail/inbox-received-detail.component";
import { InboxReceivedComponent } from "./inbox/inbox-received/inbox-received.component";
import { InboxSendedComponent } from "./inbox/inbox-sended/inbox-sended.component";
import { InboxNewComponent } from "./inbox/inbox-new/inbox-new.component";
import { InboxReceivedResolver } from "./_resolvers/inbox-received.resolver";

export const AppRoutes: Routes = [
  { path: "", component: StartComponent },
  {
    path: "",
    runGuardsAndResolvers: "always",
    canActivate: [AuthGuard],
    children: [
      {
        path: "inbox",
        component: InboxComponent,
        children: [
          {
            path: "received",
            component: InboxReceivedComponent
          },
          { path: "received/:id", component: InboxReceivedDetailComponent },
          { path: "sended", component: InboxSendedComponent },
          { path: "new", component: InboxNewComponent }
        ]
      },
      { path: "received/:id", component: InboxReceivedDetailComponent },
      { path: "projects", component: ProjectsComponent }
    ]
  },
  { path: "**", redirectTo: "", pathMatch: "full" }
];

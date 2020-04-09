import { Routes, RouterModule } from "@angular/router";
import { InboxComponent } from "./inbox.component";
import { InboxReceivedComponent } from "./inbox-received/inbox-received.component";
import { InboxReceivedDetailComponent } from "./inbox-received/inbox-received-detail/inbox-received-detail.component";
import { InboxSendedComponent } from "./inbox-sended/inbox-sended.component";
import { InboxSendedDetailComponent } from "./inbox-sended/inbox-sended-detail/inbox-sended-detail.component";
import { InboxNewComponent } from "./inbox-new/inbox-new.component";

const routes: Routes = [
  {
    path: "inbox",
    component: InboxComponent,
    children: [
      {
        path: "received",
        component: InboxReceivedComponent,
      },
      { path: "received/:id", component: InboxReceivedDetailComponent },
      { path: "sended", component: InboxSendedComponent },
      { path: "sended/:id", component: InboxSendedDetailComponent },
      { path: "new", component: InboxNewComponent },
    ],
  },
];

export const InboxRoutes = RouterModule.forChild(routes);

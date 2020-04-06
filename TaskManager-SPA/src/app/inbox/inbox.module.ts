import { NgModule } from "@angular/core";
import { InboxComponent } from "./inbox.component";
import { InboxReceivedComponent } from "./inbox-received/inbox-received.component";
import { InboxReceivedDetailComponent } from "./inbox-received/inbox-received-detail/inbox-received-detail.component";
import { InboxNewComponent } from "./inbox-new/inbox-new.component";
import { InboxSendedComponent } from "./inbox-sended/inbox-sended.component";
import { InboxSendedDetailComponent } from "./inbox-sended/inbox-sended-detail/inbox-sended-detail.component";
import { SharedModule } from "../shared/shared.module";
import { InboxRoutes } from "./inbox.routing";

@NgModule({
  declarations: [
    InboxComponent,
    InboxNewComponent,
    InboxReceivedComponent,
    InboxReceivedDetailComponent,
    InboxSendedComponent,
    InboxSendedDetailComponent,
  ],
  imports: [SharedModule, InboxRoutes],
})
export class InboxModule {}

import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { ErrorInterceptorProvider } from "./services/error.interceptor";
import { ModalModule } from "ngx-bootstrap/modal";
import { RouterModule } from "@angular/router";
import { JwtModule } from "@auth0/angular-jwt";
import { AppRoutes } from "./app.routing";
import { HttpClientModule } from "@angular/common/http";
import { InfiniteScrollModule } from "ngx-infinite-scroll";

import { AppComponent } from "./app.component";
import { LoginRegisterComponent } from "./login-register/login-register.component";
import { ErrorModalComponent } from "./helpers/error-modal/error-modal.component";
import { InboxComponent } from "./inbox/inbox.component";
import { StartComponent } from "./start/start.component";
import { InboxNewComponent } from "./inbox/inbox-new/inbox-new.component";
import { InboxReceivedComponent } from "./inbox/inbox-received/inbox-received.component";
import { InboxSendedComponent } from "./inbox/inbox-sended/inbox-sended.component";
import { ProjectsComponent } from "./projects/projects.component";
import { ProjectTasksComponent } from "./projects/project-tasks/project-tasks.component";

import { AlertModule } from "ngx-bootstrap";
import { NavComponent } from "./nav/nav.component";
import { InboxReceivedDetailComponent } from "./inbox/inbox-received/inbox-received-detail/inbox-received-detail.component";
import { InboxReceivedResolver } from "./_resolvers/inbox-received.resolver";

export function tokenGetter() {
  return localStorage.getItem("token");
}
@NgModule({
  declarations: [
    AppComponent,
    LoginRegisterComponent,
    ErrorModalComponent,
    InboxComponent,
    StartComponent,
    InboxReceivedComponent,
    InboxSendedComponent,
    InboxNewComponent,
    ProjectsComponent,
    ProjectTasksComponent,
    NavComponent,
    InboxReceivedDetailComponent
  ],
  imports: [
    AlertModule.forRoot(),
    InfiniteScrollModule,
    HttpClientModule,
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    RouterModule.forRoot(AppRoutes),
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ["localhost:5000"],
        blacklistedRoutes: ["localhost:5000/api/auth"]
      }
    }),
    ModalModule.forRoot()
  ],
  providers: [ErrorInterceptorProvider, InboxReceivedResolver],
  bootstrap: [AppComponent],
  entryComponents: [ErrorModalComponent]
})
export class AppModule {}

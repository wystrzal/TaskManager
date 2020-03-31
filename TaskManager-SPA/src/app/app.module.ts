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
import { TabsModule } from "ngx-bootstrap/tabs";
import { InfiniteScrollModule } from "ngx-infinite-scroll";

import { AppComponent } from "./app.component";
import { LoginRegisterComponent } from "./login-register/login-register.component";
import { HomeComponent } from "./home/home.component";
import { ErrorModalComponent } from "./helpers/error-modal/error-modal.component";
import { InboxComponent } from "./inbox/inbox.component";
import { StartComponent } from "./start/start.component";
import { InboxNewComponent } from "./inbox/inbox-new/inbox-new.component";
import { InboxReceivedComponent } from "./inbox/inbox-received/inbox-received.component";
import { InboxSendedComponent } from "./inbox/inbox-sended/inbox-sended.component";
import { ProjectsComponent } from "./projects/projects.component";
import { ProjectTasksComponent } from "./projects/project-tasks/project-tasks.component";

import { AlertModule } from "ngx-bootstrap";

export function tokenGetter() {
  return localStorage.getItem("token");
}
@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginRegisterComponent,
    ErrorModalComponent,
    InboxComponent,
    StartComponent,
    InboxReceivedComponent,
    InboxSendedComponent,
    InboxNewComponent,
    ProjectsComponent,
    ProjectTasksComponent
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
    ModalModule.forRoot(),
    TabsModule.forRoot()
  ],
  providers: [ErrorInterceptorProvider],
  bootstrap: [AppComponent],
  entryComponents: [ErrorModalComponent]
})
export class AppModule {}

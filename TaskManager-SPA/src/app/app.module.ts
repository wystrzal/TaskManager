import { NgModule } from "@angular/core";
import { InboxModule } from "./inbox/inbox.module";
import { SharedModule } from "./shared/shared.module";
import { ProjectsModule } from "./projects/projects.module";

import { AppComponent } from "./app.component";
import { LoginRegisterComponent } from "./login-register/login-register.component";
import { StartComponent } from "./start/start.component";
import { NavComponent } from "./nav/nav.component";
import { CoreModule } from "./core/core.module";
import { RouterModule } from "@angular/router";
import { AppRoutes } from "./app.routing";

@NgModule({
  declarations: [
    AppComponent,
    LoginRegisterComponent,
    StartComponent,
    NavComponent,
  ],
  imports: [
    RouterModule.forRoot(AppRoutes),
    SharedModule,
    CoreModule,
    InboxModule,
    ProjectsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents: [],
})
export class AppModule {}

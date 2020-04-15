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
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ImportantComponent } from "./important/important.component";

@NgModule({
  declarations: [
    AppComponent,
    LoginRegisterComponent,
    StartComponent,
    NavComponent,
    ImportantComponent,
  ],
  imports: [
    RouterModule.forRoot(AppRoutes),
    SharedModule,
    CoreModule,
    InboxModule,
    ProjectsModule,
    BrowserAnimationsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents: [],
})
export class AppModule {}

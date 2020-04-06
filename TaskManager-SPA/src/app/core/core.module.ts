import { NgModule } from "@angular/core";
import { JwtModule } from "@auth0/angular-jwt";
import { ErrorInterceptorProvider } from "./error.interceptor";
import { ErrorModalComponent } from "./helpers/error-modal/error-modal.component";
import { BrowserModule } from "@angular/platform-browser";
import { HttpClientModule } from "@angular/common/http";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { RouterModule } from "@angular/router";
import { AppRoutes } from "../app.routing";

export function tokenGetter() {
  return localStorage.getItem("token");
}
@NgModule({
  declarations: [ErrorModalComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(AppRoutes),
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ["localhost:5000"],
        blacklistedRoutes: ["localhost:5000/api/auth"]
      }
    })
  ],
  providers: [ErrorInterceptorProvider],
  entryComponents: [ErrorModalComponent],
  exports: [RouterModule]
})
export class CoreModule {}

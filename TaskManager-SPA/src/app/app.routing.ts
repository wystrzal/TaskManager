import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { AuthGuard } from "./guards/auth.guard";
import { StatisticsComponent } from "./statistics/statistics.component";

export const AppRoutes: Routes = [
  { path: "", component: HomeComponent },
  {
    path: "",
    runGuardsAndResolvers: "always",
    canActivate: [AuthGuard],
    children: [{ path: "statistics", component: StatisticsComponent }]
  },
  { path: "**", redirectTo: "", pathMatch: "full" }
];

import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { AuthService } from "../services/auth.service";
import { Observable } from "rxjs";
import { Project } from "../models/project.model";

@Injectable({
  providedIn: "root",
})
export class ProjectService {
  baseUrl =
    environment.apiUrl +
    "user/" +
    this.authService.decodedToken.nameid +
    "/project";

  constructor(private http: HttpClient, private authService: AuthService) {}

  addProject(model: any) {
    return this.http.post(this.baseUrl, model);
  }

  getProjects(type: string, skip: number): Observable<Project[]> {
    let params = new HttpParams();

    params = params.append("type", type);
    params = params.append("skip", skip.toString());

    return this.http.get<Project[]>(this.baseUrl, { params });
  }

  getProject(projectId: number): Observable<Project> {
    return this.http.get<Project>(this.baseUrl + "/" + projectId);
  }
}

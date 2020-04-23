import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { AuthService } from "../shared/services/auth.service";
import { Observable } from "rxjs";
import { Project } from "../models/project.model";
import { User } from "../models/user.model";

@Injectable({
  providedIn: "root",
})
export class ProjectService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private authService: AuthService) {}

  addProject(model: any) {
    return this.http.post(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/project",
      model
    );
  }

  getProjects(type: string, skip: number): Observable<Project[]> {
    let params = new HttpParams();
    params = params.append("type", type);
    params = params.append("skip", skip.toString());

    return this.http.get<Project[]>(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/project",
      { params }
    );
  }

  getProject(projectId: number): Observable<Project> {
    return this.http.get<Project>(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/project/" +
        projectId
    );
  }

  getProjectUsers(projectId: number): Observable<User> {
    return this.http.get<User>(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/project/" +
        projectId +
        "/users"
    );
  }

  getInvitations(): Observable<Project[]> {
    return this.http.get<Project[]>(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/project/" +
        "invitations"
    );
  }

  deleteProject(projectId: number) {
    return this.http.delete(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/project/" +
        projectId
    );
  }

  addToProject(projectId: number, userNick: string) {
    return this.http.post(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/project/" +
        projectId +
        "/new/" +
        userNick,
      {}
    );
  }

  joinToProject(projectId: number, action: number) {
    let params = new HttpParams();
    params = params.append("action", action.toString());

    return this.http.post(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/project/" +
        "join/" +
        projectId,
      {},
      { params }
    );
  }

  leaveProject(projectId: number) {
    return this.http.post(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/project/" +
        "leave/" +
        projectId,
      {}
    );
  }

  changeProjectName(projectId: number, model: any) {
    return this.http.put(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/project/" +
        "change/" +
        projectId,
      model
    );
  }
}

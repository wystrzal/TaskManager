import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Task } from "src/app/models/task.model";
import { Observable } from "rxjs";
import { AuthService } from "./auth.service";

@Injectable({
  providedIn: "root",
})
export class TaskService {
  baseUrl = environment.apiUrl;

  url(userId: number, projectId: number) {
    return this.baseUrl + "user/" + userId + "/project/" + projectId + "/task";
  }

  constructor(private http: HttpClient, private authService: AuthService) {}

  addTask(userId: number, projectId: number, model: any) {
    return this.http.post(this.url(userId, projectId), model);
  }

  getTasks(projectId: number, skip: number): Observable<Task[]> {
    let params = new HttpParams();

    params = params.append("skip", skip.toString());

    return this.http.get<Task[]>(
      this.url(this.authService.decodedToken.nameid, projectId),
      { params }
    );
  }

  getImportantTasks(skip: number): Observable<Task[]> {
    let params = new HttpParams();

    params = params.append("skip", skip.toString());

    return this.http.get<Task[]>(
      this.url(this.authService.decodedToken.nameid, 0) + "/important",
      {
        params,
      }
    );
  }

  changeStatusPriority(taskId: number, action: string, newStatPrior: any) {
    let params = new HttpParams();

    params = params.append("action", action);
    params = params.append("newStatPrior", newStatPrior);

    return this.http.put(
      this.url(this.authService.decodedToken.nameid, null) +
        "/change/" +
        taskId,
      {},
      { params }
    );
  }

  deleteTask(taskId: number) {
    return this.http.delete(
      this.url(this.authService.decodedToken.nameid, null) + "/" + taskId
    );
  }
}

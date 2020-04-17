import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Task } from "src/app/models/task.model";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class TaskService {
  baseUrl = environment.apiUrl;

  url(userId: number, projectId: number) {
    return this.baseUrl + "user/" + userId + "/project/" + projectId + "/task";
  }

  constructor(private http: HttpClient) {}

  addTask(userId: number, projectId: number, model: any) {
    return this.http.post(this.url(userId, projectId), model);
  }

  getTasks(
    userId: number,
    projectId: number,
    skip: number
  ): Observable<Task[]> {
    let params = new HttpParams();

    params = params.append("skip", skip.toString());

    return this.http.get<Task[]>(this.url(userId, projectId), { params });
  }

  getImportantTasks(userId: number, skip: number): Observable<Task[]> {
    let params = new HttpParams();

    params = params.append("skip", skip.toString());

    return this.http.get<Task[]>(this.url(userId, 0) + "/important", {
      params,
    });
  }

  changeStatusPriority(
    userId: number,
    taskId: number,
    action: string,
    newStatPrior: any
  ) {
    let params = new HttpParams();

    params = params.append("action", action);
    params = params.append("newStatPrior", newStatPrior);

    return this.http.put(
      this.url(userId, null) + "/change/" + taskId,
      {},
      { params }
    );
  }

  deleteTask(userId: number, taskId: number) {
    return this.http.delete(this.url(userId, null) + "/" + taskId);
  }
}

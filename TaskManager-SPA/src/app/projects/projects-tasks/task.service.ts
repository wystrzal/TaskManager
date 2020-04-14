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

  getTasks(userId: number, projectId: number): Observable<Task[]> {
    return this.http.get<Task[]>(this.url(userId, projectId));
  }
}

import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient } from "@angular/common/http";

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
}

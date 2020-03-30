import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Observable } from "rxjs";
import { User } from "../models/user.model";

@Injectable({
  providedIn: "root"
})
export class UserService {
  baseUrl = environment.apiUrl + "user/";

  constructor(private http: HttpClient) {}

  getUser(userId: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + "nick/" + userId);
  }
}

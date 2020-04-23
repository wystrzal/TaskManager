import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Observable } from "rxjs";
import { User } from "../models/user.model";
import { AuthService } from "../shared/services/auth.service";

@Injectable({
  providedIn: "root",
})
export class UserService {
  baseUrl = environment.apiUrl + "user";

  constructor(private http: HttpClient, private authService: AuthService) {}

  getUser(): Observable<User> {
    return this.http.get<User>(
      this.baseUrl + "/" + this.authService.decodedToken.nameid
    );
  }

  changePhoto(photoId: number) {
    return this.http.put(
      this.baseUrl +
        "/" +
        this.authService.decodedToken.nameid +
        "/photo/" +
        photoId,
      {}
    );
  }

  changeNick(nickModel: any) {
    return this.http.put(
      this.baseUrl + "/" + this.authService.decodedToken.nameid + "/changeNick",
      nickModel
    );
  }

  changePassword(passwordModel: any) {
    return this.http.put(
      this.baseUrl +
        "/" +
        this.authService.decodedToken.nameid +
        "/changePassword",
      passwordModel
    );
  }
}

import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable, Subject } from "rxjs";
import { Message } from "../models/message.model";
import { AuthService } from "../services/auth.service";

@Injectable({
  providedIn: "root"
})
export class MessageService {
  userId = this.authService.decodedToken.nameid;
  baseUrl = environment.apiUrl + "message/user/" + this.userId;
  newMessage = new Subject<void>();

  constructor(private http: HttpClient, private authService: AuthService) {}

  getReceivedMessages(skip: number): Observable<Message[]> {
    let params = new HttpParams();

    params = params.append("skip", skip.toString());

    return this.http.get<Message[]>(this.baseUrl + "/received", {
      params
    });
  }

  getSendedMessages(skip: number): Observable<Message[]> {
    let params = new HttpParams();

    params = params.append("skip", skip.toString());

    return this.http.get<Message[]>(this.baseUrl + "/sended", {
      params
    });
  }

  getMessage(messageId: number): Observable<Message> {
    return this.http.get<Message>(this.baseUrl + "/" + messageId);
  }

  sendMessage(recipientNick: string, model: any) {
    let params = new HttpParams();

    params = params.append("recipientNick", recipientNick);

    return this.http.post(this.baseUrl + "/send", model, { params });
  }

  deleteMessage(messageId: number, userType: string) {
    let params = new HttpParams();

    params = params.append("userType", userType);

    return this.http.post(
      this.baseUrl + "/delete/" + messageId,
      {},
      { params }
    );
  }
}

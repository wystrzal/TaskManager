import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable, Subject } from "rxjs";
import { Message } from "../models/message.model";
import { AuthService } from "../shared/services/auth.service";

@Injectable({
  providedIn: "root",
})
export class MessageService {
  baseUrl = environment.apiUrl;
  newMessage = new Subject<void>();

  constructor(private http: HttpClient, private authService: AuthService) {}

  getReceivedMessages(skip: number): Observable<Message[]> {
    let params = new HttpParams();

    params = params.append("skip", skip.toString());

    return this.http.get<Message[]>(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/message/" +
        "received",
      {
        params,
      }
    );
  }

  getSendedMessages(skip: number): Observable<Message[]> {
    let params = new HttpParams();

    params = params.append("skip", skip.toString());

    return this.http.get<Message[]>(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/message/" +
        "sended",
      {
        params,
      }
    );
  }

  getMessage(messageId: number): Observable<Message> {
    return this.http.get<Message>(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/message/" +
        messageId
    );
  }

  sendMessage(recipientNick: string, model: any) {
    let params = new HttpParams();

    params = params.append("recipientNick", recipientNick);

    return this.http.post(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/message/" +
        "send",
      model,
      { params }
    );
  }

  deleteMessage(messageId: number, userType: string) {
    let params = new HttpParams();

    params = params.append("userType", userType);

    return this.http.post(
      this.baseUrl +
        "user/" +
        this.authService.decodedToken.nameid +
        "/message/" +
        "delete/" +
        messageId,
      {},
      { params }
    );
  }
}

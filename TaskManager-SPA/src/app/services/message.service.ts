import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable, Subject } from "rxjs";
import { Message } from "../models/message.model";

@Injectable({
  providedIn: "root"
})
export class MessageService {
  baseUrl = environment.apiUrl + "message/";
  newMessage = new Subject<void>();

  constructor(private http: HttpClient) {}

  getReceivedMessages(userId: number, skip: number): Observable<Message[]> {
    let params = new HttpParams();

    params = params.append("skip", skip.toString());

    return this.http.get<Message[]>(this.baseUrl + "received/" + userId, {
      params
    });
  }

  getSendedMessages(userId: number, skip: number): Observable<Message[]> {
    let params = new HttpParams();

    params = params.append("skip", skip.toString());

    return this.http.get<Message[]>(this.baseUrl + "sended/" + userId, {
      params
    });
  }

  getMessage(messageId: number, userId: number): Observable<Message> {
    return this.http.get<Message>(this.baseUrl + messageId + "/user/" + userId);
  }

  sendMessage(userId: number, recipientNick: string, model: any) {
    let params = new HttpParams();

    params = params.append("recipientNick", recipientNick);

    return this.http.post(this.baseUrl + userId, model, { params });
  }

  deleteMessage(messageId: number, userId: number, userType: string) {
    let params = new HttpParams();

    params = params.append("userType", userType);

    return this.http.post(
      this.baseUrl + "delete/" + messageId + "/user/" + userId,
      {},
      { params }
    );
  }
}

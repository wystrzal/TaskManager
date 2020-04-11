import { Injectable } from "@angular/core";
import { BsModalService, BsModalRef } from "ngx-bootstrap/modal";
import { ErrorModalComponent } from "./error-modal/error-modal.component";

@Injectable({
  providedIn: "root",
})
export class ErrorService {
  bsModalRef: BsModalRef;
  constructor(private modalService: BsModalService) {}

  newError(error: string) {
    this.openErrorModal(error);
  }

  confirm(message: string, action: any) {
    this.openErrorModal(null, action, message);
  }

  openErrorModal(isError?: string, isAction?: any, isMessage?: string) {
    const initialState = {
      error: isError,
      action: isAction,
      message: isMessage,
    };
    this.bsModalRef = this.modalService.show(ErrorModalComponent, {
      initialState,
    });
  }
}

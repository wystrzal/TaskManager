import { Injectable } from "@angular/core";
import { BsModalService, BsModalRef } from "ngx-bootstrap/modal";
import { ErrorModalComponent } from "../helpers/error-modal/error-modal.component";

@Injectable({
  providedIn: "root"
})
export class ErrorService {
  error: string;
  id: number;
  line: string;

  bsModalRef: BsModalRef;
  constructor(private modalService: BsModalService) {}

  newError(error: string) {
    this.error = error;
    this.openModalWithComponent();
  }

  delete(id: number) {}

  openModalWithComponent() {
    const initialState = {
      error: this.error,
      id: this.id
    };
    this.bsModalRef = this.modalService.show(ErrorModalComponent, {
      initialState
    });
  }
}

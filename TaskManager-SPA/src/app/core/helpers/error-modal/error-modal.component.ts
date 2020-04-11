import { Component, OnInit } from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";

@Component({
  selector: "app-add-modal",
  templateUrl: "./error-modal.component.html",
  styleUrls: ["./error-modal.component.css"],
})
export class ErrorModalComponent implements OnInit {
  error: string;
  id: number;
  message: string;
  action: any;

  constructor(public bsModalRef: BsModalRef) {}

  confirmAction() {
    this.action();
    this.bsModalRef.hide();
  }

  ngOnInit() {}
}

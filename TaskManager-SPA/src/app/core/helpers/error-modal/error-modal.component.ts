import { Component, OnInit } from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";

@Component({
  selector: "app-add-modal",
  templateUrl: "./error-modal.component.html",
  styleUrls: ["./error-modal.component.css"]
})
export class ErrorModalComponent implements OnInit {
  error: string;
  id: number;

  constructor(public bsModalRef: BsModalRef) {}

  confirmDelete(event: MouseEvent) {}

  ngOnInit() {}
}

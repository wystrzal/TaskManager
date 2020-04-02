import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-inbox",
  templateUrl: "./inbox.component.html",
  styleUrls: ["./inbox.component.css"]
})
export class InboxComponent implements OnInit {
  routerValue: string;

  constructor() {}

  ngOnInit() {}
}

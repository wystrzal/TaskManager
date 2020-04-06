import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { AlertModule, ModalModule } from "ngx-bootstrap";
import { InfiniteScrollModule } from "ngx-infinite-scroll";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";

@NgModule({
  imports: [
    CommonModule,
    AlertModule.forRoot(),
    InfiniteScrollModule,
    ModalModule.forRoot(),
    FormsModule,
    RouterModule
  ],
  declarations: [],
  exports: [
    CommonModule,
    AlertModule,
    InfiniteScrollModule,
    ModalModule,
    FormsModule,
    RouterModule
  ]
})
export class SharedModule {}

import { Component, OnInit } from "@angular/core";
import { PaginatorState } from "primeng/paginator";
import { BaseComponent } from "./base.component";
import { Pager, SteamStat } from "./models";
import { Perform } from "./perform";
import { StreamerService } from "./streamer.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
})
export class AppComponent extends BaseComponent implements OnInit {
  protected first = 1;
  protected rows = 5;

  protected perform = new Perform<Pager<SteamStat>>();

  constructor(private streamerService: StreamerService) {
    super();
  }

  ngOnInit(): void {
    this.getStats(this.first, this.rows);
  }

  protected onPageChange(event: PaginatorState): void {
    this.first = event.first ?? 1;
    this.rows = event.rows ?? 5;

    this.getStats(event.page ? event.page + 1 : 1, this.rows);
  }

  private getStats(pageNumber: number, pageSize: number): void {
    this.safeSub(
      this.perform
        .load(this.streamerService.getStats(pageNumber, pageSize))
        .subscribe()
    );
  }
}

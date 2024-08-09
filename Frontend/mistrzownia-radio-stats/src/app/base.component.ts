import { Injectable, OnDestroy } from "@angular/core";
import { Subscription } from "rxjs";

@Injectable()
export abstract class BaseComponent implements OnDestroy {
  private subscriptions: Subscription[] = [];

  ngOnDestroy(): void {
    this.subscriptions
      .filter((sub: Subscription) => sub != null)
      .forEach((sub: Subscription) => sub.unsubscribe());
  }

  protected safeSub(...sub: Subscription[]): void {
    this.subscriptions = this.subscriptions.concat(sub);
  }
}

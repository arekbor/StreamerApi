import {
  Observable,
  catchError,
  finalize,
  of,
  switchMap,
  throwError,
} from "rxjs";

export class Perform<T> {
  data: T | null;
  isLoading = false;
  hasError = false;

  public load(
    action$: Observable<T>,
    resetStatementsOnFinalize = true
  ): Observable<T> {
    this.data = null;
    this.isLoading = true;
    this.hasError = false;

    return action$.pipe(
      switchMap((data: T) => {
        this.data = data;
        return of(data);
      }),
      catchError((err) => {
        this.data = null;
        this.isLoading = false;
        this.hasError = true;
        return throwError(() => err);
      }),
      finalize(() => {
        if (resetStatementsOnFinalize) {
          this.isLoading = false;
          this.hasError = false;
        }
      })
    );
  }
}

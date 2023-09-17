import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoadingController } from '@ionic/angular';
import {
  Observable,
  delay,
  retryWhen,
  tap,
  map,
  catchError,
  EMPTY,
  finalize,
  throwError,
} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoaderService implements HttpInterceptor {
  constructor(private loadingCtrl: LoadingController) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    this.loadingCtrl.getTop().then((hasLoading) => {
      if (!hasLoading) {
        this.loadingCtrl
          .create({
            spinner: 'circular',
            translucent: true,
          })
          .then((loading) => loading.present());
      }
    });

    return next.handle(req).pipe(
      catchError((err) => {
        return throwError(err);
      }),
      finalize(() => {
        this.loadingCtrl.getTop().then((hasLoading) => {
          if (hasLoading) this.loadingCtrl.dismiss();
        });
      })
    );
  }
}

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
  catchError,
  finalize,
  from,
  switchMap,
  tap,
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
    let loading: HTMLIonLoadingElement;

    return from(
      this.loadingCtrl.create({
        spinner: 'circular',
        translucent: true,
      })
    ).pipe(
      tap((loadingElement) => {
        loading = loadingElement;
        loading.present();
      }),
      switchMap(() => next.handle(req)),
      finalize(() => loading.dismiss())
    );
  }
}

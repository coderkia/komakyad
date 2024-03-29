import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept(
        req: import('@angular/common/http').HttpRequest<any>,
        next: import('@angular/common/http').HttpHandler):
        import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {
                console.log(error);
                if (error.status === 401) {
                    return throwError(error.error || error.statusText);
                }
                if (error.status === 403) {
                    return throwError(error.error || error.statusText);
                }
                if (error instanceof HttpErrorResponse) {
                    const applicationError = error.headers.get('Application-Error');
                    if (applicationError) {
                        return throwError(applicationError);
                    }
                    const serverError = error.error;
                    let modelStateErrors = '';
                    if (serverError.errors && typeof serverError.errors === 'object') {
                        for (const key in serverError.errors) {
                            if (serverError.errors[key]) {
                                modelStateErrors += serverError.errors[key] + '<br/>';
                            }
                        }
                    }
                    let userIdentityErrors = '';
                    if (serverError && Array.isArray(serverError)) {
                        for (const item of serverError) {
                            userIdentityErrors += item.description + '\r\n';
                        }
                    }
                    return throwError(modelStateErrors || userIdentityErrors || serverError || 'Server Error');
                }
            })
        );
    }

}

export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};

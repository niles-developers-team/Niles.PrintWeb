import { HttpInterceptor, HttpRequest, HttpEvent, HttpHandler } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class ApiUrlInterceptor implements HttpInterceptor {
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const { url } = request;
        const prefixRegex = /^\/?api\//;
        const apiUrl = 'http://localhost:5000'
        let newUrl: string;
        const matches = prefixRegex.exec(url);
        if (matches && matches.length > 0) {
            const offset = matches[0].length;
            newUrl = `${apiUrl}/api/${url.substring(offset)}`;
        }

        request = request.clone({
            url: newUrl
        });

        console.log(`new api url: ${request.url}`);

        return next.handle(request);
    }
}
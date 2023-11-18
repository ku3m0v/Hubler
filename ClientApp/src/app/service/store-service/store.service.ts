import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {catchError} from 'rxjs/operators';
import configurl from "../../../assets/config/config.json";
import {AuthenticationService} from "../auth-service/authentication.service";

export interface SupermarketWithAddress {
    id: number;
    title: string;
    phone: number;
    street: string;
    house: number;
    city: string;
    postalCode: string;
    country: string;
}

@Injectable({
    providedIn: 'root'
})
export class SupermarketService {
    private myAppUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.myAppUrl = configurl.apiServer.url;
  }

  getAllSupermarkets(): Observable<SupermarketWithAddress[]> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
    return this.http.get<SupermarketWithAddress[]>(`${this.myAppUrl}api/supermarket/list`, { headers: headers })
      .pipe(catchError(this.errorHandler));
  }



  insertSupermarket(supermarket: SupermarketWithAddress): Observable<any> {
        return this.http.post(`${this.myAppUrl}api/supermarket/insert`, supermarket)
            .pipe(
                catchError(this.errorHandler)
            );
    }

    getSupermarketByTitle(title: string): Observable<SupermarketWithAddress> {
        return this.http.get<SupermarketWithAddress>(`${this.myAppUrl}api/supermarket/get/${title}`)
            .pipe(
                catchError(this.errorHandler)
            );
    }

    updateSupermarket(supermarket: SupermarketWithAddress): Observable<any> {
        return this.http.post(`${this.myAppUrl}api/supermarket/update`, supermarket)
            .pipe(
                catchError(this.errorHandler)
            );
    }

    deleteSupermarket(title: string): Observable<any> {
        return this.http.delete(`${this.myAppUrl}api/supermarket/delete?title=${title}`)
            .pipe(
                catchError(this.errorHandler)
            );
    }

    private errorHandler(error: HttpErrorResponse) {
        console.error('An error occurred:', error.message);
        return throwError(error);
    }
}

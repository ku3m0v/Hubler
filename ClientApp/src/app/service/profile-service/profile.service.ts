import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import configurl from "../../../assets/config/config.json";
import { AuthenticationService } from "../auth-service/authentication.service";

export interface ProfileData {
  email: string;
  firstName: string;
  lastName: string;
  createdDate: Date;
  supermarketName?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private myAppUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.myAppUrl = configurl.apiServer.url;
  }

  getProfile(): Observable<ProfileData> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
    return this.http.get<ProfileData>(`${this.myAppUrl}api/profile`, { headers: headers })
      .pipe(catchError(this.errorHandler));
  }

  updateProfile(profile: ProfileData): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
    return this.http.put(`${this.myAppUrl}api/profile`, profile, { headers: headers, responseType: 'text' })
      .pipe(catchError(this.errorHandler));
  }

  uploadPhoto(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file, file.name);

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });

    return this.http.post(`${this.myAppUrl}api/profile/upload-photo`, formData, { headers: headers, responseType: 'text' })
      .pipe(catchError(this.errorHandler));
  }

  getProfilePicture(): Observable<Blob> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
    return this.http.get(`${this.myAppUrl}api/profile/profile-picture`, { headers: headers, responseType: 'blob' })
      .pipe(catchError(this.errorHandler));
  }


  private errorHandler(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(error);
  }
}

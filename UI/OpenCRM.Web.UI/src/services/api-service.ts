import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private apiUrl = 'http://localhost:5005/api';
  constructor(private http: HttpClient) {}

  sendGet(relativeUrl: string): Observable<any> {
    const fillUrl = this.apiUrl + relativeUrl;
    return this.http.get(fillUrl);
  }

  sendPost(relativeUrl: string, body: any): Observable<any> {
    try {
      const fillUrl = this.apiUrl + relativeUrl;
      return this.http.post(fillUrl, body);
    } catch (error) {
      console.error('Error in sendPost:', error);
      throw error;
    }
  }
}

import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IItemListingResponse } from '../models/item-listing-response.dto';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { HttpResponse } from '@capacitor/core';
import { IItemAddRequest } from '../models/item-add-reqest.dto';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  BASE_URL = `${environment.SERVER_URL}/item`;

  constructor(private http: HttpClient) {}

  getAllSelling(): Observable<IItemListingResponse[]> {
    return this.http.get<IItemListingResponse[]>(`${this.BASE_URL}/forsale`);
  }

  getAllBuying(): Observable<IItemListingResponse[]> {
    return this.http.get<IItemListingResponse[]>(`${this.BASE_URL}/tobuy`);
  }

  getBoughtItems(): Observable<IItemListingResponse[]> {
    return this.http.get<IItemListingResponse[]>(`${this.BASE_URL}/bought`);
  }

  getSoldItems(): Observable<IItemListingResponse[]> {
    return this.http.get<IItemListingResponse[]>(`${this.BASE_URL}/sold`);
  }

  buyItem(id: number): Observable<HttpResponse> {
    return this.http.patch<HttpResponse>(`${this.BASE_URL}/${id}`, {});
  }

  deleteItem(id: number): Observable<HttpResponse> {
    return this.http.delete<HttpResponse>(`${this.BASE_URL}/${id}`);
  }

  addItem(item: IItemAddRequest): Observable<HttpResponse> {
    console.log(item);
    return this.http.post<HttpResponse>(`${this.BASE_URL}`, item);
  }
}

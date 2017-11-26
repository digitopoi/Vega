import { Injectable } from '@angular/core';
import { Inject } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class VehicleService {

  private BASE_URL: string = 'localhost:5000';

  constructor(private http: Http) {
  }

  getMakes() {
    return this.http.get(this.BASE_URL + '/api/makes')
      .map(res => res.json());
  }

  getFeatures() {
    return this.http.get(this.BASE_URL + '/api/features')
      .map(res => res.json());
  }

  create(vehicle: any) {
    return this.http.post(this.BASE_URL + '/api/vehicles', vehicle)
      .map(res => res.json());
  }
}

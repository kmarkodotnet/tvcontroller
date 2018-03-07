import { Injectable } from '@angular/core';
//import { Observable } from 'rxjs/Observable';
//import { Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../appSettings';

@Injectable()
export class ShutDownService {

  public SHUTDOWN_ENDPOINT= AppSettings.API_ENDPOINT + "shutdown/";

  public constructor(private http: HttpClient) { }
  
  public shutDown():void
  {
    this.http.get(this.SHUTDOWN_ENDPOINT).toPromise();
  }
}
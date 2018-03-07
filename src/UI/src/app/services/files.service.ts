import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { EntriesData } from '../model/entriesData.model';
import { Entry } from '../model//entry.model';
import { State } from '../model//state.model';
import { AppSettings } from '../appSettings';

@Injectable()
export class FilesService {

  public FILES_ENDPOINT= AppSettings.API_ENDPOINT + "files/";

  public constructor(private http: HttpClient) { }

  ed : EntriesData;
  
  public getFiles(path:string):Promise<EntriesData> 
  {
    return this.http.get(this.FILES_ENDPOINT+'listEntries?requestDir='+path).toPromise().then(f => f as EntriesData);
  }

  public getState():Promise<State> 
  {
    return this.http.get(this.FILES_ENDPOINT+'getState').toPromise().then(f => f as State);
  }

  public play(movie:string):Promise<State> 
  {
    return this.http.get(this.FILES_ENDPOINT+'play?moviePath='+movie+'&subtitlePath=no').toPromise().then(f => f as State);
  }
  
  public continue():Promise<State> 
  {
    return this.http.get(this.FILES_ENDPOINT+'continue').toPromise().then(f => f as State);
  }

  public pause():Promise<State> 
  {
    return this.http.get(this.FILES_ENDPOINT+'pause').toPromise().then(f => f as State);
  }

  public forward():Promise<State> 
  {
    return this.http.get(this.FILES_ENDPOINT+'forward').toPromise().then(f => f as State);
  }
  
  public backward():Promise<State> 
  {
    return this.http.get(this.FILES_ENDPOINT+'backward').toPromise().then(f => f as State);
  }

  public stop():Promise<State> 
  {
    return this.http.get(this.FILES_ENDPOINT+'stop').toPromise().then(f => f as State);
  }

  public playSubtitle(movie:string,subtitle:string):void
  {
    this.http.get(this.FILES_ENDPOINT+'play?moviePath='+movie+'&subtitlePath=no');
  }
}
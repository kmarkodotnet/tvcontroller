import { Component, Input } from '@angular/core';
import { ShutDownService } from '../services/shutdown.service';
import { FilesService } from '../services/files.service';
import { Entry } from '../model/entry.model';
import { State } from '../model//state.model';

@Component({
  selector: 'functions',
  templateUrl: './functions.component.html',
  styleUrls: ['./functions.component.css'
  ]
})
export class FunctionsComponent {
  
  @Input() entry: Entry
  public state: State;

  constructor(private sds: ShutDownService, private fs: FilesService){
    this.fs.getState().then((result) => {this.state = result; console.log(this.state);});
  }
  
  public playClick(){
    if(this.state.MovieTitle!="" && this.state.MovieTitle!=null){
      this.fs.continue().then((result) => {this.state = result; console.log(this.state);});
    }else{
      this.fs.play(this.entry.FullName).then((result) => {this.state = result; });
    }
  }

  public pauseClick(){
    this.fs.pause().then((result) => {this.state = result; });
  }
  public forwardClick(){
    this.fs.forward().then((result) => {this.state = result; });
  }
  public backwardClick(){
    this.fs.backward().then((result) => {this.state = result; });
  }
  public stopClick(){
    this.fs.stop().then((result) => {this.state = result; console.log(result);});
  }
  public turnOffClick(){
    this.sds.shutDown();
  }

  
}

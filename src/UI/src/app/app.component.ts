import { Component } from '@angular/core';
import { Entry } from './model/entry.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css','../../node_modules/angular-material/angular-material.min.css']
})
export class AppComponent {
  title = 'TV Controller';
  selectedEntry: Entry;
  //selectedEntry: Entry[];
}

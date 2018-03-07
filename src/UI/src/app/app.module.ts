import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatListModule } from '@angular/material';
import {MatCardModule} from '@angular/material/card';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { FunctionsComponent } from './functions/functions.component';
import { FilesListComponent } from './filesList/filesList.component';
import { FilesService } from './services/files.service';
import { ShutDownService } from './services/shutdown.service';

@NgModule({
  declarations: [
    AppComponent,
    FunctionsComponent,
    FilesListComponent
  ],
  imports: [
    NgbModule.forRoot(),
    BrowserModule,
    HttpClientModule,
    MatButtonModule,
    MatGridListModule,
    MatListModule,
    MatCardModule
  ],
  providers: [FilesService,ShutDownService],
  bootstrap: [AppComponent]
})
export class AppModule { }

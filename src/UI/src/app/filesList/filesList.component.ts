import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { FilesService } from '../services/files.service';
import { EntriesData } from '../model/entriesData.model';
import { Entry } from '../model/entry.model';
import {MatSelectionList} from '@angular/material/list';

@Component({
  selector: 'filesList',
  templateUrl: './filesList.component.html',
  styleUrls: ['./filesList.component.css']
})
export class FilesListComponent implements OnInit {

  public entriesData  : EntriesData;

  @ViewChild(MatSelectionList) entries: MatSelectionList;

  @Input() selected: Entry;
  @Output() selectedChange: EventEmitter<Entry> = new EventEmitter();

  constructor(private fs: FilesService){
    this.entriesData = new EntriesData();
  }
  
  public ngOnInit(): void {
    this.fs.getFiles("").then((result) => this.entriesData = result);

  }

  public onRowClicked(item: Entry){
    
    this.selectedChange.next(item);
    if(item.EntryType==1 || item.EntryType==0){
      this.fs.getFiles(item.FullName).then((result) => this.entriesData = result);
    }
  }
}
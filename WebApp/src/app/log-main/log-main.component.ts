import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { WebAPIService } from 'src/app/service/web-api.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Subscription } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { PaginationComponent } from '../pagination/pagination.component';
declare var $: any;

@Component({
  selector: 'app-log-main',
  templateUrl: './log-main.component.html',
  styleUrls: ['./log-main.component.scss']
})

export class LogMainComponent implements OnInit, OnDestroy {
  @ViewChild(PaginationComponent) paginationComponent: PaginationComponent;
  public LogIdPicked;
  private logTypeNgModel = 'Info';
  private statusNgModel = 'Open';
  private descriptionNgModel;
  private logs;
  private filterSearch;
  private filteredOption;
  private isHidden: boolean = false;
  private myControl = new FormControl();
  private userID = localStorage.getItem('userID');

  private httpSubscription: Subscription = new Subscription();

  constructor(private _service: WebAPIService) {
  }

  ngOnInit() {
    this.paginationComponent.controller = 'Logs';
    this.paginationComponent.PageInit();
    this.GetAllLogForSearch();
    this.filteredOption = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => value.length > 0 ? this.Filter(value.toString()) : []));
  }

  ngOnDestroy() {
    this.httpSubscription.unsubscribe();
  }

  private setId(id) {
    this.LogIdPicked = id;
  }

  private Filter(value: string): string {
    const filterValue = value.toLowerCase();
    return this.filterSearch.filter(result => result.toLowerCase().includes(filterValue));
  }

  private GetAllLogForSearch() {
    this.httpSubscription.add(this._service.FetchAll('Logs/GetAllLogForSearch')
      .subscribe(result => this.filterSearch = result));
  }

  private SearchLog() {
    this.paginationComponent.dataSearch = this.myControl.value;
    let dataSearch: string = this.myControl.value;
    if (dataSearch === '') {
      this.paginationComponent.PageInit();
    }
    else {
      this.paginationComponent.currentPage = 1;
      this.paginationComponent.RefeshePageArray();
      this.paginationComponent.GetDataForPagination();
    }
  }

  private OnSubmit(formAddLog) {
    if (!formAddLog.invalid) {
      this.httpSubscription.add(this._service.Add('Logs/' + this.userID, formAddLog.value)
        .subscribe(result => {
          this.paginationComponent.PageInit();
          $('#logName').val('');
          this.logTypeNgModel = 'Info';
          this.statusNgModel = 'Open'
          this.descriptionNgModel = '';
          $('#CreateLogModal').modal('hide');
        }));
    }
  }

  private DeleteLog(logID) {
    this.httpSubscription.add(
      this._service.Delete('Logs', logID)
        .subscribe(success => {
          this.paginationComponent.RefeshePageArray();
        }));
  }

  private RefeshArray(array: any[]) {
    this.logs = array;
    if (array) {
      this.isHidden = false;
    }
    else {
      this.isHidden = true;
    }
  }

  editorConfig: AngularEditorConfig = {
    editable: true,
    height: '25px'
  };
}
import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';
import { WebAPIService } from 'src/app/service/web-api.service';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent implements OnInit, OnDestroy {
  
  @Output() private sendArray = new EventEmitter();
  public controller: string = '';
  public dataSearch: string = '';
  public pageSize: number = 10;
  private pageNumber;
  private pageArray = [];
  public currentPage: number;
  private httpSubscription: Subscription = new Subscription();

  constructor(private _service: WebAPIService) { }

  ngOnInit() {

  }

  ngOnDestroy() {
    this.httpSubscription.unsubscribe();
  }

  public RefeshePageArray() {
    if (this.dataSearch === '') {
      this.httpSubscription.add(this._service.FetchAll(this.controller + '/GetCountTotal')
        .subscribe(result => {
          switch(this.controller){
            case 'Logs':
            this.pageNumber = (result[0].TotalLogs % this.pageSize === 0 ? result[0].TotalLogs / this.pageSize : parseInt((result[0].TotalLogs / this.pageSize) + 1 + ""));
            break;
            case 'Users':
            this.pageNumber = (result[0].TotalUsers % this.pageSize === 0 ? result[0].TotalUsers / this.pageSize : parseInt((result[0].TotalUsers / this.pageSize) + 1 + ""));
            break;
            case 'Roles':
            this.pageNumber = (result[0].TotalRoles % this.pageSize === 0 ? result[0].TotalRoles / this.pageSize : parseInt((result[0].TotalRoles / this.pageSize) + 1 + ""));
            break;
          }

          this.pageArray = [];

          if (this.currentPage == 1) {
            for (let i = 1; i <= this.pageNumber && i <= 5; i++) {
              this.pageArray.push(i)
            }
          }
          else if (this.currentPage >= this.pageNumber) {
            this.currentPage = this.pageNumber;
            for (let i = this.pageNumber; i >= this.pageNumber - 4 && i > 0; i--) {
              this.pageArray.unshift(i)
            }
          }
          else {
            for (let i = this.pageNumber; i >= this.pageNumber - 4 && i > 0; i--) {
              this.pageArray.unshift(i)
            }
          }
          this.GetDataForPagination();
        }));
    }
    else {
      this.httpSubscription.add(this._service.FetchAll(this.controller + '/CountResultSearch?dataSearch=' + this.dataSearch)
        .subscribe(result => {
          let countResult: number = parseInt(result + "");
          this.pageNumber = (countResult % this.pageSize === 0 ? countResult / this.pageSize : parseInt((countResult / this.pageSize) + 1 + ""));
          this.pageArray = [];

          if (this.currentPage == 1) {
            for (let i = 1; i <= this.pageNumber && i <= 5; i++) {
              this.pageArray.push(i)
            }
          }
          else if (this.currentPage >= this.pageNumber) {
            this.currentPage = this.pageNumber;
            for (let i = this.pageNumber; i >= this.pageNumber - 4 && i > 0; i--) {
              this.pageArray.unshift(i)
            }
          }
          else {
            for (let i = this.pageNumber; i >= this.pageNumber - 4 && i > 0; i--) {
              this.pageArray.unshift(i)
            }
          }
          this.GetDataForPagination();
        }));
    }
  }

  public PageInit() {
    this.currentPage = 1;
    this.RefeshePageArray();
    this.GetDataForPagination();
  }

  public ClickFirstPage() {
    if (this.currentPage > 1) {
      this.currentPage = 1;
      this.RefeshePageArray();
      this.GetDataForPagination();
    }
  }

  public ClickPreviousPage() {
    if (this.currentPage > 1) {
      if (this.pageArray.indexOf(this.currentPage) == 0) {
        this.pageArray.pop();
        this.pageArray.unshift(this.currentPage - 1);
      }
      this.currentPage--;
      this.GetDataForPagination();
    }
  }

  public ClickNextPage() {
    if (this.currentPage < this.pageNumber) {
      if (this.pageArray.indexOf(this.currentPage) == 4) {
        this.pageArray.shift();
        this.pageArray.push(this.currentPage + 1);
      }
      this.currentPage++;
      this.GetDataForPagination();
    }
  }

  public ClickLastPage() {
    if (this.currentPage < this.pageNumber) {
      this.currentPage = this.pageNumber;
      this.RefeshePageArray();
      this.GetDataForPagination();
    }
  }

  public ClickPageNumber(pageClicked: number) {
    if (pageClicked != this.currentPage) {
      this.currentPage = pageClicked;
      this.GetDataForPagination();
    }
  }

  public GetDataForPagination() {
    if (this.dataSearch === '') {
      this.httpSubscription.add(this._service.GetAllWithSkips(this.controller + '/GetAllDataForPagination', (this.currentPage - 1) * this.pageSize, this.pageSize, this.dataSearch)
        .subscribe(result => this.sendArray.emit(result)));
    }
    else {
      this.httpSubscription.add(this._service.GetAllWithSkips(this.controller + '/Search/', (this.currentPage - 1) * this.pageSize, this.pageSize, this.dataSearch)
        .subscribe(result => this.sendArray.emit(result)));
    }
  }

}

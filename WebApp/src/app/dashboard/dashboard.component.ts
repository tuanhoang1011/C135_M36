import { Component, OnInit } from '@angular/core';
import { WebAPIService } from 'src/app/service/web-api.service';

declare var Chart:any;

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],

})


export class DashboardComponent implements OnInit {
    //count the number of role, user and log
    dataTotal;
    newCount:number;

    //All Data of logs
    LogOverview;
    Logs;

    //Percent of Log Data
    openPercent:number=0;

    processPercent:number=0;
    donePercent:number=0;

    //Daily log
    dailyLog;
    Labels=[];
    Data=[];

    /**
     * Get total of users, logs and roles
     */
    GetCountTotal():void{
        this._service.FetchAll('Logs/GetCountTotal').subscribe(x=>{
            //Adapt data to our data
            this.dataTotal = x[0];
            this.openPercent=(this.dataTotal.TotalLogsOpen*100)/this.dataTotal.TotalLogs;
            this.processPercent=(this.dataTotal.TotalLogsProcessing*100)/this.dataTotal.TotalLogs;
            this.donePercent=(this.dataTotal.TotalLogsDone*100)/this.dataTotal.TotalLogs;
        });
    }

    /**
     * Get daily logs in week and draw chart
     */
    GetLogDaily(){
        this._service.FetchAll('Logs/GetCountWeek').subscribe(x=>{
            this.dailyLog=x;
            this.dailyLog.forEach(i => {
                i.dayweek= new Date(i.dayweek);
                i.dayweek = i.dayweek.getDate()+"/"+i.dayweek.getMonth();
                this.Labels.unshift(i.dayweek);
                this.Data.unshift(i.totallogs);
            });
            this.newCount=this.Data[this.Data.length-1];
            this.DrawChart();
        });
    }

    /**
     * Get log overview
     */
    GetAllLog():void{
        this._service.FetchAll('logs/GetLogOverview').subscribe(x=>{
            /*Process the LOG DATA */
            this.LogOverview=x;//this.LogOverview now have all logs
        });
    }

    //Draw chart using ChartJS
    DrawChart():void {
        var ctx = (< HTMLCanvasElement >document.getElementById("myChart")).getContext('2d');
        var myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: this.Labels,
                datasets: [{
                    label: 'Daily Logs',
                    data: this.Data,
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero:true
                        }
                    }]
                }
            }
        });
    }

    constructor(private _service:WebAPIService){}

    ngOnInit() {
        //Count total of users, logs, roles
        this.GetCountTotal();

        //show all overview logs
        this.GetAllLog();

        //Count daily logs and draw the chart
        this.GetLogDaily();
    }

}

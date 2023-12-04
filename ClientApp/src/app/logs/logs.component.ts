import { Component, OnInit } from '@angular/core';
import {Log, LogService} from "../service/logs-service/log.service";

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css']
})
export class LogsComponent implements OnInit {
  logs: Log[] = [];
  showSpinner = true;
  showMsg = false;

  constructor(private logService: LogService) {}

  ngOnInit(): void {
    this.fetchLogs();
    setTimeout(() => {
      this.showSpinner = false;
      this.showMsg = true;
    }, 1500);
  }

  fetchLogs(): void {
    this.logService.getAllLogs().subscribe(
      (data: Log[]) => {
        this.logs = data;
      },
      (error) => {
        console.error('Error fetching logs:', error);
      }
    );
  }

  downloadLogs(): void {
    this.logService.downloadCsv().subscribe(
      (data: Blob) => {
        const a = document.createElement('a');
        const objectUrl = URL.createObjectURL(data);
        a.href = objectUrl;
        a.download = 'logs.csv';
        a.click();
        URL.revokeObjectURL(objectUrl);
      },
      (error) => {
        console.error('Error downloading the file:', error);
      }
    );
  }
}

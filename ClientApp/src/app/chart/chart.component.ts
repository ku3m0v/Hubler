import { Component, AfterViewInit, ElementRef } from '@angular/core';

declare var ApexCharts: any; // Assumes ApexCharts is globally available

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements AfterViewInit {

  private options: any; // Use a specific type if you have a type for the options

  constructor(private elRef: ElementRef) {
    this.options = {
      series: [
        {
          name: "Developer Edition",
          data: [1500, 1418, 1456, 1526, 1356, 1256],
          color: "#1A56DB",
        },
        {
          name: "Designer Edition",
          data: [643, 413, 765, 412, 1423, 1731],
          color: "#7E3BF2",
        },
      ],
      chart: {
        height: "100%",
        maxWidth: "100%",
        type: "area",
        fontFamily: "Inter, sans-serif",
        dropShadow: {
          enabled: false,
        },
        toolbar: {
          show: false,
        },
      },
      tooltip: {
        enabled: true,
        x: {
          show: false,
        },
      },
      legend: {
        show: false
      },
      fill: {
        type: "gradient",
        gradient: {
          opacityFrom: 0.55,
          opacityTo: 0,
          shade: "#1C64F2",
          gradientToColors: ["#1C64F2"],
        },
      },
      dataLabels: {
        enabled: false,
      },
      stroke: {
        width: 6,
      },
      grid: {
        show: false,
        strokeDashArray: 4,
        padding: {
          left: 2,
          right: 2,
          top: 0
        },
      },
      xaxis: {
        categories: ['01 February', '02 February', '03 February', '04 February', '05 February', '06 February', '07 February'],
        labels: {
          show: false,
        },
        axisBorder: {
          show: false,
        },
        axisTicks: {
          show: false,
        },
      },
      yaxis: {
        show: false,
        labels: {
          formatter: (value: number) => '$' + value
        }
      },
    };
  }

  ngAfterViewInit(): void {
    this.initChart();
  }

  private initChart(): void {
    const chartContainer = this.elRef.nativeElement.querySelector('#data-series-chart');
    if (chartContainer && ApexCharts) {
      const chart = new ApexCharts(chartContainer, this.options);
      chart.render();
    }
  }
}

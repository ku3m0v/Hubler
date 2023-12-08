import {AfterViewInit, Component, ElementRef, OnInit} from '@angular/core';
import {AuthenticationService} from "../service/auth-service/authentication.service";
import {
  ExpiredInventory,
  ExpiredWarehouse,
  SupermarketSalesSummary,
  Top5ProductsBySupermarket,
  ViewsService
} from "../service/view-service/views.service";
import {FormControl} from "@angular/forms";

declare var ApexCharts: any;

interface Product {
  imageUrl: string;
  id: number;
  title: string;
  date?: string;
  soldQuantity?: number;
}

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements AfterViewInit, OnInit {
  expiredInventories: ExpiredInventory[] = [];
  expiredWarehouses: ExpiredWarehouse[] = [];
  top5Products: Top5ProductsBySupermarket[] = [];
  salesSummaries: SupermarketSalesSummary[] = [];
  supermarketTitles: string[] = [];
  selectedSupermarketTitle: string = '';
  loading = true;
  private options: any;

  constructor(private elRef: ElementRef, private authService: AuthenticationService, private viewsService: ViewsService) {
    // this.options = {
    //   chart: {
    //     height: "100%",
    //     maxWidth: "100%",
    //     type: "area",
    //     fontFamily: "Inter, sans-serif",
    //     dropShadow: {
    //       enabled: false,
    //     },
    //     toolbar: {
    //       show: false,
    //     },
    //   },
    //   tooltip: {
    //     enabled: true,
    //     x: {
    //       show: false,
    //     },
    //   },
    //   fill: {
    //     type: "gradient",
    //     gradient: {
    //       opacityFrom: 0.65,
    //       opacityTo: 0,
    //       shade: "#ffd400",
    //       gradientToColors: ["#ff9200"],
    //     },
    //   },
    //   dataLabels: {
    //     enabled: false,
    //   },
    //   stroke: {
    //     width: 6,
    //   },
    //   grid: {
    //     show: false,
    //     strokeDashArray: 4,
    //     padding: {
    //       left: 2,
    //       right: 2,
    //       top: 0
    //     },
    //   },
    //   series: [{
    //     name: "Total Sales",
    //     data: []  // Data will be set dynamically
    //   }],
    //   xaxis: {
    //     categories: ['1.1.23', '1.2.23', '1.3.23', '1.4.23', '1.5.23', '1.5.23', '1.6.23', '1.7.23', '1.10.23', '1.11.23', '1.12.23', '1.13.23'],
    //     labels: {
    //       show: false,
    //     },
    //     axisBorder: {
    //       show: false,
    //     },
    //     axisTicks: {
    //       show: false,
    //     },
    //   },
    //   yaxis: {
    //     show: false,
    //   },
    // };
  }

  ngOnInit(): void {
    this.fetchSupermarketTitles();
  }


  fetchSupermarketTitles(): void {
    this.viewsService.getSupermarketTitles().subscribe(
      (titles: string[]) => {
        this.supermarketTitles = titles;
      },
      (error) => console.error('Error fetching supermarket titles:', error)
    );
  }

  onSelectSupermarketTitle(title: string): void {
    this.selectedSupermarketTitle = title;
    this.fetchData();
  }

  fetchData(): void {
    if (!this.selectedSupermarketTitle) {
      console.error('Supermarket title is required');
      return;
    }

    this.viewsService.getExpiredInventory(this.selectedSupermarketTitle).subscribe(
      (data: ExpiredInventory[]) => this.expiredInventories = data,
      (error) => console.error('Error fetching expired inventories:', error)
    );

    this.viewsService.getExpiredWarehouse(this.selectedSupermarketTitle).subscribe(
      (data: ExpiredWarehouse[]) => this.expiredWarehouses = data,
      (error) => console.error('Error fetching expired warehouses:', error)
    );

    this.viewsService.getTop5ProductsBySupermarket(this.selectedSupermarketTitle).subscribe(
      (data: Top5ProductsBySupermarket[]) => this.top5Products = data,
      (error) => console.error('Error fetching top 5 products:', error)
    );

    this.viewsService.getSupermarketSalesSummary(this.selectedSupermarketTitle).subscribe(
      (data: SupermarketSalesSummary[]) => {
        this.salesSummaries = data;
        // Update chart data if applicable
        this.loading = false;
      },
      (error) => {
        console.error('Error fetching sales summaries:', error);
        this.loading = false;
      }
    );
  }
  // private updateChartData(data: SupermarketSalesSummary[]): void {
  //   this.options.series[0].data = data.map(item => item.totalSales);
  //   this.options.xaxis.categories = data.map(item => `${item.month}/${item.year}`);
  //   // Re-initialize the chart to update data
  //   this.initChart();
  // }

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

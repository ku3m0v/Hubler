import {AfterViewInit, Component, ElementRef, OnInit} from '@angular/core';
import {AuthenticationService} from "../service/auth-service/authentication.service";
import {
  ExpiredInventory,
  ExpiredWarehouse,
  SupermarketSalesSummary,
  Top5ProductsBySupermarket,
  ViewsService
} from "../service/view-service/views.service";

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
  loading = true;

  selectedValue: string = 'Last 7 days';
  isDropdownVisible: boolean = false;
  salesValue: string = '12,423';
  percentageChange: number = 23;
  topProducts: Product[] = [
    {
      imageUrl: 'https://flowbite.s3.amazonaws.com/docs/gallery/masonry/image-3.jpg',
      id: 1,
      title: 'Product 1',
      soldQuantity: 100
    },
    {
      imageUrl: 'https://flowbite.s3.amazonaws.com/docs/gallery/masonry/image-5.jpg',
      id: 2,
      title: 'Product 2',
      soldQuantity: 100
    },
    {
      imageUrl: 'https://flowbite.s3.amazonaws.com/docs/gallery/masonry/image-6.jpg',
      id: 3,
      title: 'Product 3',
      soldQuantity: 100
    },
  ];
  expiredProducts: Product[] = [
    {
      imageUrl: 'https://flowbite.s3.amazonaws.com/docs/gallery/masonry/image-6.jpg',
      id: 1,
      title: 'Fresh Peaches',
      date: '2021/02/01'
    },
    {
      imageUrl: 'https://flowbite.s3.amazonaws.com/docs/gallery/masonry/image-3.jpg',
      id: 2,
      title: 'Corn Onion Muffaleta Bread',
      date: '2021/02/01'
    },
    {
      imageUrl: 'https://flowbite.s3.amazonaws.com/docs/gallery/masonry/image-3.jpg',
      id: 3,
      title: 'Fresh Blueberries',
      date: '2021/02/03'
    },
    {
      imageUrl: 'https://flowbite.s3.amazonaws.com/docs/gallery/masonry/image-3.jpg',
      id: 4,
      title: 'Fresh Blueberries',
      date: '2021/02/03'
    },
    {
      imageUrl: 'https://flowbite.s3.amazonaws.com/docs/gallery/masonry/image-3.jpg',
      id: 5,
      title: 'Fresh Blueberries',
      date: '2021/02/03'
    },
  ];
  private options: any;

  constructor(private elRef: ElementRef, private authService: AuthenticationService, private viewsService: ViewsService) {
    this.options = {
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
      fill: {
        type: "gradient",
        gradient: {
          opacityFrom: 0.65,
          opacityTo: 0,
          shade: "#ffd400",
          gradientToColors: ["#ff9200"],
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
      series: [{
        name: "Total Sales",
        data: []  // Data will be set dynamically
      }],
      xaxis: {
        categories: ['1.1.23', '1.2.23', '1.3.23', '1.4.23', '1.5.23', '1.5.23', '1.6.23', '1.7.23', '1.10.23', '1.11.23', '1.12.23', '1.13.23'],
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
      },
    };
  }

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData(): void {
    this.viewsService.getExpiredInventory().subscribe(
      (data: ExpiredInventory[]) => this.expiredInventories = data,
      (error) => console.error('Error fetching expired inventories:', error)
    );

    this.viewsService.getExpiredWarehouse().subscribe(
      (data: ExpiredWarehouse[]) => this.expiredWarehouses = data,
      (error) => console.error('Error fetching expired warehouses:', error)
    );

    this.viewsService.getTop5ProductsBySupermarket().subscribe(
      (data: Top5ProductsBySupermarket[]) => this.top5Products = data,
      (error) => console.error('Error fetching top 5 products:', error)
    );

    this.viewsService.getSupermarketSalesSummary().subscribe(
      (data: SupermarketSalesSummary[]) => {
        this.salesSummaries = data;
        // this.updateChartData(data);
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

import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  showLoading: boolean = true;
  showGallery: boolean = false;

  ngOnInit() {
    setTimeout(() => {
      this.showLoading = false;
      this.showGallery = true;
    }, 1000);
  }

}

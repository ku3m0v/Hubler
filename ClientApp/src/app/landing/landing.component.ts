import {Component, ElementRef} from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
    selector: 'app-landing',
    templateUrl: './landing.component.html',
    styleUrls: ['./landing.component.css']
})
export class LandingComponent {

    constructor(private route: ActivatedRoute, private el: ElementRef) {
    }

    ngAfterViewInit() {
        this.route.fragment.subscribe(f => {
            const element = this.el.nativeElement.querySelector('#' + f);
            if (element) element.scrollIntoView({behavior: 'smooth'});
        });
    }

}

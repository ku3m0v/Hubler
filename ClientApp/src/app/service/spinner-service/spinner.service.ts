import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class SpinnerService {
    private loading = new BehaviorSubject<boolean>(true);

    get isLoading() {
        return this.loading.asObservable();
    }

    setLoading(isLoading: boolean) {
        this.loading.next(isLoading);
    }
}

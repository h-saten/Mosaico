import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { timezones } from '../data/timezone';
import { Timezone } from '../models/timezone';
import { SuccessResponse } from '../utils/success-response';

@Injectable({
    providedIn: 'root'
})
export class TimezoneService {

    getTimezones(): Observable<SuccessResponse<Timezone[]>> {
        return of({ok: true, data: timezones});
    }

}

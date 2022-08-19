import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { countries } from '../data/countries';
import { Country } from '../models/country';
import { SuccessResponse } from '../utils/success-response';

@Injectable({
    providedIn: 'root'
})
export class CountryService {

    getCountries(): Observable<SuccessResponse<Country[]>> {
        return of({ok: true, data: countries});
    }

}

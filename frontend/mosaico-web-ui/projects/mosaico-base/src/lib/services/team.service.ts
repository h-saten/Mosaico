import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { teams } from '../data/teams';
import { Team } from '../models/team';
import { SuccessResponse } from '../utils/success-response';

@Injectable({
    providedIn: 'root'
})
export class TeamService {

    getTeamMembers(): Observable<SuccessResponse<Team[]>> {
        return of({ok: true, data: teams});
    }

}

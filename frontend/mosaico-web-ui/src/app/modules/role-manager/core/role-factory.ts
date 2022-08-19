import { Observable, of } from 'rxjs';
import { InjectionToken } from '@angular/core';

export function defaultRoleFactory(): Observable<string[]> {
    return of([]);
}

export const RoleFactory = new InjectionToken<Function>('roleFactory');
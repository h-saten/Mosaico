import {Inject, Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, Resolve} from '@angular/router';
import {DOCUMENT} from '@angular/common';

@Injectable()
export class LoginResolve implements Resolve<any> {
  constructor(
    @Inject(DOCUMENT) private document: Document
  ) {}

  resolve(route: ActivatedRouteSnapshot) {
    return;
  }
}

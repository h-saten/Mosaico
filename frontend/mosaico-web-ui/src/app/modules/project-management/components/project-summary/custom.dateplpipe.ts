import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';
@Pipe({
    name: 'myDatePl',
  })
  export class MyDatePlPipe extends DatePipe implements PipeTransform {

  transform(value: any, format?: string, timezone?: string, locale: string = 'pl-PL'): string | any {

    // const currentParams = [value, format, timezone, locale];

    return super.transform(value, "dd MMMM yyyy, HH:mm'", timezone, locale);

  }
}

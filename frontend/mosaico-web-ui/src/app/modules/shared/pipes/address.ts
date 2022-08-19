import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'addressType'})
export class AddressPipe implements PipeTransform {

  transform(value: string, number: number = 6): string {
    if (!value) {
      return "";
    }
    if(!number || number <= 0){
      number = 6;
    }
    return (value.slice(0, number) + "..." + value.slice(-4));
  }
}


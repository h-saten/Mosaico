import { HttpHeaders } from '@angular/common/http';

export const DefaultHeaders: HttpHeaders = new HttpHeaders({
    'Content-Type': 'application/json'
});

export const FileHeaders: HttpHeaders = new HttpHeaders({
    'Content-Type': 'multipart/form-data; charset=utf-8'
});

export const PdfHeaders: HttpHeaders = new HttpHeaders({
    'Content-Type': 'application/pdf'
});
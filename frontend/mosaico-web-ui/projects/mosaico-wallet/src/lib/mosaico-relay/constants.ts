import { HttpHeaders } from "@angular/common/http";

export const RelayHeaders: HttpHeaders = new HttpHeaders({
    'Content-Type': 'application/json',
    'Auth-Type': "id-token"
});
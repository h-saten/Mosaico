import { HttpHeaders } from "@angular/common/http";

export const DefaultHeaders = {
    headers: new HttpHeaders({
        "Accept": "application/json",
        "Content-Type": "application/json"
    })
}
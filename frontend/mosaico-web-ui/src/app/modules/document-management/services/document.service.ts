import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AddDocumentContentCommand } from './../commands';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService, DefaultHeaders, SuccessResponse } from "mosaico-base";

@Injectable({ providedIn: 'root' })
export class DocumentService {

  private baseUrl = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getConfig().gatewayUrl;
  }

  addDocumentContents(documentId: string, command: AddDocumentContentCommand): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/documents/${documentId}/contents`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  removeDocumentContents(documentId: string, language: string): Observable<SuccessResponse<null>> {
    return this.http.delete<SuccessResponse<null>>(`${this.baseUrl}/core/api/documents/${documentId}/contents/${language}`, { headers: DefaultHeaders });
  }

  storeFile(files: FileList): Observable<SuccessResponse<string>> {
    const file: File = files[0];
    const formData = new FormData();
    formData.append('files', file, file.name);
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/documents/files`, formData, { headers: headers });
  }
}

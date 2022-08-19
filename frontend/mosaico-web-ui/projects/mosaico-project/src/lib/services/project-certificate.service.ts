import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {
  base64toBlob,
  ConfigService,
  DefaultHeaders,
  FileContentResult,
  FileResponse,
  SuccessResponse
} from 'mosaico-base';
import {CertificateConfigurationResponse} from "../responses";
import {UpsertCertificateConfigurationCommand} from "../commands";
import {map} from "rxjs/operators";

@Injectable({
    providedIn: 'root'
})
export class ProjectCertificateService {

    private baseUrl = '';

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getCertificateConfiguration(projectId: string, language: string): Observable<SuccessResponse<CertificateConfigurationResponse>> {
      return this.http.get<SuccessResponse<CertificateConfigurationResponse>>(`${this.baseUrl}/core/api/projects/${projectId}/certificate/configuration?language=${language}`, { headers: DefaultHeaders });
    }

    upsertCertificateConfiguration(projectId: string, command: UpsertCertificateConfigurationCommand): Observable<SuccessResponse<object>> {
        return this.http.post<SuccessResponse<object>>(`${this.baseUrl}/core/api/projects/${projectId}/certificate/configuration`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    uploadCertificateBackground(projectId: string, fileToUpload: File, language: string): Observable<SuccessResponse<string>> {
        const endpoint = `${this.baseUrl}/core/api/projects/${projectId}/certificate/background`;
        const formData: FormData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
        formData.append('language', language);
        return this.http.post<SuccessResponse<string>>(endpoint, formData);
    }

    getExampleCertificatePdf(projectId: string, language: string): Observable<SuccessResponse<FileResponse>> {
      const responseBeforeBlobMapping = this.http.get<SuccessResponse<FileContentResult>>(`${this.baseUrl}/core/api/projects/${projectId}/certificate/example?language=${language}`, { headers: DefaultHeaders });
      return responseBeforeBlobMapping.pipe<SuccessResponse<FileResponse>>(map((response) => {
        return {
          data: {
            data: base64toBlob(response.data.fileContents, response.data.contentType),
            fileName: response.data.fileDownloadName,
          } as FileResponse,
          ok: response.ok
        }
      }));
    }

    getUserCertificatePdf(projectId: string, language: string): Observable<SuccessResponse<FileResponse>> {
      const responseBeforeBlobMapping = this.http.get<SuccessResponse<FileContentResult>>(`${this.baseUrl}/core/api/projects/${projectId}/certificate?language=${language}`, { headers: DefaultHeaders });
      return responseBeforeBlobMapping.pipe<SuccessResponse<FileResponse>>(map((response) => {
        return {
          data: {
            data: base64toBlob(response.data.fileContents, response.data.contentType),
            fileName: response.data.fileDownloadName,
          } as FileResponse,
          ok: response.ok
        }
      }));
    }
}

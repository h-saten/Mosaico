import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {
  ChangeEmail,
  ChangePassword,
  ConfirmChangePassword,
  DeactivateUserCommand,
  GenerateSmsConfirmationCodeCommand,
  UpdateUserCommand
} from '../commands';
import {ConfigService, DefaultHeaders, SuccessResponse} from 'mosaico-base';
import {
  EvaluationQuestion,
  GetPhoneNumberExistenceResponse,
  GetUserProfilePermissionsResponse,
  UserInformation
} from '../models';
import {UpdatePhoneNumberCommand} from '../commands';
import {VerifyPhoneNumberCommand} from "../commands/verify-phone-number.command";

@Injectable({
    providedIn: 'root'
  })
export class UserService {
    private baseUrl = "";

    constructor(private http: HttpClient, configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    public getUser(id: string): Observable<SuccessResponse<UserInformation>> {
        return this.http.get<SuccessResponse<UserInformation>>(`${this.baseUrl}/id/api/users/${id}`, { headers: DefaultHeaders});
    }

    public getUsers(first: number, rows: number, name: string = null, email: string = null): Observable<SuccessResponse<any>> {
      let params = new HttpParams();
      if (first) params = params.append('skip', first);
      if (rows) params = params.append('take', rows);
      if (name) params = params.append('firstName', name);
      if (email) params = params.append('email', email);
      return this.http.get<SuccessResponse<any>>(`${this.baseUrl}/id/api/users`, { headers: DefaultHeaders, params});
    }

    public deactivateUser(command:DeactivateUserCommand): Observable<SuccessResponse<any>> {
      return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/id/api/users/Deactivate`, JSON.stringify(command), { headers: DefaultHeaders});
    }

    public initKyc(command: any): Observable<SuccessResponse<string>> {
        return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/id/api/users/self/kyc`, JSON.stringify(command), { headers: DefaultHeaders});
    }

    public checkEmailExistence(email:string):Observable<SuccessResponse<any>>{
        return this.http.get<SuccessResponse<any>>(`${this.baseUrl}/id/api/auth/ApiAccount/CheckEmailExistence?email=${email}`, { headers: DefaultHeaders});
    }

    public updateUser(id: string, command: UpdateUserCommand): Observable<SuccessResponse<any>> {
      return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/id/api/users/${id}`, JSON.stringify(command), { headers: DefaultHeaders});
    }

    public ChangeEmail(id: string, command: ChangeEmail): Observable<SuccessResponse<any>> {
      return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/id/api/users/${id}/email`, JSON.stringify(command), { headers: DefaultHeaders});
    }

    public ChangePassword(command: ChangePassword): Observable<SuccessResponse<any>> {
      return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/id/api/auth/ApiAccount/ChangePassword`, JSON.stringify(command), { headers: DefaultHeaders});
    }

    public ConfirmChangePassword(command: ConfirmChangePassword): Observable<SuccessResponse<any>> {
      return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/id/api/auth/ApiAccount/ConfirmChangePassword`, JSON.stringify(command), { headers: DefaultHeaders});
    }

    public sendDeleteUserRequest(id: string, password: string): Observable<any>{
        return this.http.delete<SuccessResponse<boolean>>(`${this.baseUrl}/id/api/users/${id}`, {
            headers: DefaultHeaders,
            body: JSON.stringify({password})
        });
    }

    public checkPhoneExistence(phoneNumber: string): Observable<SuccessResponse<GetPhoneNumberExistenceResponse>> {
      return this.http.get<SuccessResponse<GetPhoneNumberExistenceResponse>>(`${this.baseUrl}/id/api/auth/ApiAccount/CheckPhoneNumberExistence?phoneNumber=${encodeURIComponent(phoneNumber)}`, {
        headers: DefaultHeaders,
      });
    }

    public sendPhoneNumberVerificationCode(id: string, command: GenerateSmsConfirmationCodeCommand): Observable<SuccessResponse<string>> {
      return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/id/api/users/${id}/phone-number-confirmation/generate-code`, JSON.stringify(command), { headers: DefaultHeaders});
    }

    public verifyPhoneNumber(id: string, command: VerifyPhoneNumberCommand): Observable<SuccessResponse<string>> {
      return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/id/api/users/${id}/phone-number-confirmation/verify`, JSON.stringify(command), { headers: DefaultHeaders});
    }

    public updateLanguage(language: string): Observable<SuccessResponse<any>>{
        return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/id/api/users/self/language`, JSON.stringify({language}), { headers: DefaultHeaders});
    }

    public updateUserPhoto(file: File, id: string): Observable<SuccessResponse<string>> {
        const formData = new FormData();
        formData.append('file', file, file.name);
        let headers = new HttpHeaders();
        headers.append('Content-Type', 'multipart/form-data');
        return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/users/${id}/photo`, formData, { headers: headers });
    }

    public deleteUserPhoto(id: string): Observable<SuccessResponse<any>> {
        return this.http.delete<SuccessResponse<any>>(`${this.baseUrl}/core/api/users/${id}/photo`, { headers: DefaultHeaders });
    }

    public updatePhoneNumber(command: UpdatePhoneNumberCommand): Observable<SuccessResponse<any>> {
      return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/id/api/users/self/phone-number`, JSON.stringify(command), { headers: DefaultHeaders});
    }

    public getProfilePermissions(): Observable<SuccessResponse<GetUserProfilePermissionsResponse>> {
      return this.http.get<SuccessResponse<GetUserProfilePermissionsResponse>>(`${this.baseUrl}/id/api/users/self/profile/permissions`, {
        headers: DefaultHeaders,
      });
    }

    public completeEvaluation(responses: EvaluationQuestion[]): Observable<SuccessResponse<any>> {
      return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/id/api/users/self/evaluation`, JSON.stringify({responses}), { headers: DefaultHeaders});
    }

    public getKycStatus(): Observable<SuccessResponse<string>>{
      return this.http.get<SuccessResponse<string>>(`${this.baseUrl}/id/api/users/self/kyc`, { headers: DefaultHeaders});
    }
}

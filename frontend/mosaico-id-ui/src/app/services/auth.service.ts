import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { DefaultHeaders, SuccessResponse } from "../utils";
import { AppConfigurationService } from "./app-configuration.service";

export interface InitiateForgotPassword {
    email: string;
}

export interface ResetPassword {
    email: string;
    password: string;
    confirmPassword: string;
    code: string;
}

export interface ChangeEmail {
  userId: string;
  email: string;
  code: string;
}

export interface GetEmailExistenceResponse {
    exist: boolean;
}

export interface GetPhoneNumberExistenceResponse {
    exist: boolean;
}

export interface LoginCommand {
    email: string;
    password: string;
    remember: boolean;
    returnUrl: string;
    authorizeDeviceCode: string
}

export interface LoginCommandResponse {
    result: LoginResult;
}

export enum DeviceAuthorizationType {
  email = "Email",
  sms = 'Sms'
}

export interface DeviceAuthorization {
  deviceVerificationType: DeviceAuthorizationType;
  codeExpiryAt: Date | null;
  failureReason: string;
  canGenerateNewCode: boolean;
  lastGeneratedCodeStillValid: boolean;
}

export interface LoginResult {
  defaultRedirect: string;
  redirectAfterLoginUrl: string;
  type: LoginResponseType;
  deviceVerificationRequired: boolean;
  deviceAuthorization: DeviceAuthorization
}

export interface LoginResult {
    defaultRedirect: string;
    redirectAfterLoginUrl: string;
    type: LoginResponseType;
    deviceVerificationType: DeviceAuthorizationType
}

export interface CreateUserCommand {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
    terms: boolean;
    notForbiddenCitizenship: boolean;
    newsletterPersonalDataProcessing: boolean;
    returnUrl: string;
    language: string;
    phoneNumber: string;
    phoneNumberConfirmationCode: string;
}

export enum LoginResponseType {
    Succeeded = "Succeeded",
    LockedOut = "LockedOut",
    RequiresTwoFactor = "RequiresTwoFactor",
    InvalidData = "InvalidData",
    Deactivated = "Deactivated",
    Error = "Error",
}

export interface LogoutCommand {
    logoutId: string;
    returnUrl?: string;
}

export interface LogoutUserResponse {
    postLogoutRedirectUri: string;
    clientName: string;
    signOutIframeUrl: string;
}

export interface RecaptchaVerificationCommand{
  response:string;
}
export interface GenerateSmsConfirmationCodeCommand {
  phoneNumber: string;
}
export interface ReportStolenCommand{
  id:string,
  code:string
}

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    baseUrl: string;

    constructor(private httpClient: HttpClient, private configService: AppConfigurationService) {
        this.baseUrl = '';
    }

    public initiateForgotPassword(command: InitiateForgotPassword): Observable<SuccessResponse<any>> {
        return this.httpClient.post<SuccessResponse<any>>(`${this.baseUrl}/api/auth/ApiAccount/ForgotPassword`, JSON.stringify(command), DefaultHeaders);
    }

    public resetPassword(command: ResetPassword): Observable<SuccessResponse<any>> {
        return this.httpClient.post<SuccessResponse<any>>(`${this.baseUrl}/api/auth/ApiAccount/ResetPassword`, JSON.stringify(command), DefaultHeaders);
    }

    public changeEmail(command: ChangeEmail): Observable<SuccessResponse<any>> {
      return this.httpClient.put<SuccessResponse<any>>(`${this.baseUrl}/api/users/${command.userId}/email`, JSON.stringify(command), DefaultHeaders);
    }

    public reportStolenAccount(command:ReportStolenCommand): Observable<SuccessResponse<any>> {
      return this.httpClient.post<SuccessResponse<any>>(`${this.baseUrl}/api/users/${command.id}/report-stolen?code=${command.code}`, { headers: DefaultHeaders});
    }

    public checkEmailExistence(email: string): Observable<SuccessResponse<GetEmailExistenceResponse>> {
        return this.httpClient.get<SuccessResponse<GetEmailExistenceResponse>>(`${this.baseUrl}/api/auth/ApiAccount/CheckEmailExistence?email=${email}`, DefaultHeaders);
    }

    public checkPhoneExistence(phoneNumber: string): Observable<SuccessResponse<GetPhoneNumberExistenceResponse>> {
        return this.httpClient.get<SuccessResponse<GetEmailExistenceResponse>>(`${this.baseUrl}/api/auth/ApiAccount/CheckPhoneNumberExistence?phoneNumber=${encodeURIComponent(phoneNumber)}`, DefaultHeaders);
    }

    public confirmEmail(userId: string, code: string): Observable<SuccessResponse<any>> {
        return this.httpClient.get<SuccessResponse<any>>(`${this.baseUrl}/api/auth/ApiAccount/ConfirmEmail?userId=${userId}&code=${code}`, DefaultHeaders);
    }

    public login(command: LoginCommand): Observable<SuccessResponse<LoginCommandResponse>> {
        return this.httpClient.post<SuccessResponse<LoginCommandResponse>>(`${this.baseUrl}/api/auth/ApiAccount/Login`, JSON.stringify(command), DefaultHeaders);
    }

    public register(command: CreateUserCommand): Observable<SuccessResponse<string>> {
        return this.httpClient.post<SuccessResponse<string>>(`${this.baseUrl}/api/auth/ApiAccount/Register`, JSON.stringify(command), DefaultHeaders);
    }

    public registerConfirmed(command: CreateUserCommand): Observable<SuccessResponse<string>> {
        return this.httpClient.post<SuccessResponse<string>>(`${this.baseUrl}/api/auth/ApiAccount/RegisterConfirmed`, JSON.stringify(command), DefaultHeaders);
    }

    public accountExists(email: string): Observable<SuccessResponse<boolean>> {
        return this.httpClient.get<SuccessResponse<boolean>>(`${this.baseUrl}/api/auth/ApiAccount/AccountExist?email=${email}`, DefaultHeaders);
    }

    public logout(command: LogoutCommand):  Observable<SuccessResponse<LogoutUserResponse>> {
        return this.httpClient.post<SuccessResponse<LogoutUserResponse>>(`${this.baseUrl}/api/auth/ApiAccount/Logout`, JSON.stringify(command), DefaultHeaders);
    }

    public sendPhoneNumberVerificationCode(command: GenerateSmsConfirmationCodeCommand): Observable<SuccessResponse<string>> {
        return this.httpClient.post<SuccessResponse<string>>(`${this.baseUrl}/api/auth/ApiAccount/SendPhoneNumberVerificationCode`, JSON.stringify(command), DefaultHeaders);
    }

    public verifyReCaptchaToken(command : RecaptchaVerificationCommand):Observable<SuccessResponse<JSON>>{
      return this.httpClient.post<SuccessResponse<JSON>>(`${this.baseUrl}/api/auth/ApiAccount/RecaptchaVerification?response=${command.response}`, DefaultHeaders);
    }
}

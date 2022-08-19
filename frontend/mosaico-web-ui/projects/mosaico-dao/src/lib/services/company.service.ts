import { CreateCompanyCommand } from '../commands/create-company.command';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, } from 'rxjs';
import { CompanyHolder, CompanyList, CompanyPermissions, Proposal, TeamMember, Verification, CompanyDocumentContent } from '../models';
import { AddProposalCommand, CreateTeamMemberCommand, DeleteUserSubscribedCompanyCommand, SetUserSubscribedCompanyCommand, UpdateCompanyCommand, UpdateCompanySocialLinks, VoteCommand, ModifyCompanyDocumentCommand,GetExportedPdfCommand, EditCompanyDocumentContentsCommand, UploadCompanyDocumentCommand } from '../commands';
import { CreateCompanyResponse, GetCompanyResponse, GetVerificationResponse, GetCompanyDocumentsResponse, GetCompanyDocumentTypesResponse } from '../responses';
import { Store } from '@ngrx/store';
import { ConfigService, DefaultHeaders, PaginationResponse, SuccessResponse,PdfHeaders } from 'mosaico-base';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private baseUrl = '';

  constructor(private http: HttpClient, private configService: ConfigService, store: Store) {
    this.baseUrl = configService.getConfig().gatewayUrl;
  }

  createCompany(command: CreateCompanyCommand): Observable<SuccessResponse<CreateCompanyResponse>> {
    return this.http.post<SuccessResponse<CreateCompanyResponse>>(`${this.baseUrl}/core/api/dao`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  uploadCompanyLogo(companyId: string, fileToUpload: File): Observable<SuccessResponse<string>> {
    const endpoint = `${this.baseUrl}/core/api/dao/${companyId}/logo`;
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(endpoint, formData, { headers });
  }

  resendInvitation(id: string, invitationId: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/core/api/dao/${id}/members/${invitationId}/resend`, null, { headers: DefaultHeaders });
  }

  acceptInvitation(code: string): Observable<any> {
    return this.http.put(`${this.baseUrl}/core/api/dao/invitation`, JSON.stringify({ code }), { headers: DefaultHeaders });
  }

  getCompany(id: string): Observable<SuccessResponse<GetCompanyResponse>> {
    return this.http.get<SuccessResponse<GetCompanyResponse>>(`${this.baseUrl}/core/api/dao/${id}`, { headers: DefaultHeaders });
  }

  leaveCompany(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/core/api/dao/${id}`, { headers: DefaultHeaders });
  }

  addVerification(companyId: any,verification: Verification): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/dao/${companyId}/verification`, verification, { headers: DefaultHeaders });
  }

  getVerification(id: string): Observable<SuccessResponse<GetVerificationResponse>> {
    return this.http.get<SuccessResponse<GetVerificationResponse>>(`${this.baseUrl}/core/api/dao/${id}/verification`, { headers: DefaultHeaders });
  }

  approveCompany(id: any): Observable<any> {
    return this.http.delete(`${this.baseUrl}/core/api/dao/${id}/verification`, { headers: DefaultHeaders });
  }

  getAllVerifications(skip: number = 0, take: number = 10): Observable<SuccessResponse<PaginationResponse<Verification>>> {
    return this.http.get<SuccessResponse<PaginationResponse<Verification>>>(`${this.baseUrl}/core/api/dao/verification?skip=${skip}&take=${take}`, { headers: DefaultHeaders });
  }

  getCompanies(skip: number = 0, take: number = 10): Observable<SuccessResponse<PaginationResponse<CompanyList>>> {
    return this.http.get<SuccessResponse<PaginationResponse<CompanyList>>>(`${this.baseUrl}/core/api/dao?skip=${skip}&take=${take}`, { headers: DefaultHeaders });
  }

  getCompaniesListPublicly(skip: number = 0, take: number = 2,search=null): Observable<SuccessResponse<PaginationResponse<CompanyList>>> {
    return this.http.get<SuccessResponse<PaginationResponse<CompanyList>>>(`${this.baseUrl}/core/api/dao/public-list?skip=${skip}&take=${take}&Search=${search}`, { headers: DefaultHeaders });
  }

  getUserCompanies(skip: number = 0, take: number = 10, userId: string): Observable<SuccessResponse<PaginationResponse<CompanyList>>> {
    return this.http.get<SuccessResponse<PaginationResponse<CompanyList>>>(`${this.baseUrl}/core/api/dao?skip=${skip}&take=${take}&userId=${userId}`, { headers: DefaultHeaders });
  }

  update(id: string, command: UpdateCompanyCommand): Observable<SuccessResponse<object>> {
    return this.http.put<SuccessResponse<object>>(`${this.baseUrl}/core/api/dao/${id}`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  getCompanyPermissions(id: string | null, userId?: string): Observable<SuccessResponse<CompanyPermissions>> {
    if(userId && userId.length > 0){
      return this.http.get<SuccessResponse<CompanyPermissions>>(`${this.baseUrl}/core/api/dao/${id}/permissions?userId=${userId}`, { headers: DefaultHeaders });
    }
    else {
      return this.http.get<SuccessResponse<CompanyPermissions>>(`${this.baseUrl}/core/api/dao/${id}/permissions`, { headers: DefaultHeaders });
    }
  }

  getCompanyTeam(id: any): Observable<SuccessResponse<PaginationResponse<TeamMember>>> {
    return this.http.get<SuccessResponse<PaginationResponse<TeamMember>>>(`${this.baseUrl}/core/api/dao/${id}/members`, { headers: DefaultHeaders });
  }

  deleteTeamMember(id: string, teamMemberId: string): Observable<SuccessResponse<any>> {
    return this.http.delete<SuccessResponse<any>>(`${this.baseUrl}/core/api/dao/${id}/members/${teamMemberId}`, { headers: DefaultHeaders });
  }

  createTeamMember(id: string, command: CreateTeamMemberCommand): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/dao/${id}/members`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  updateTeamMember(id: string, memberId: string, role: string): Observable<SuccessResponse<any>> {
    return this.http.put<SuccessResponse<string>>(`${this.baseUrl}/core/api/dao/${id}/members/${memberId}`, JSON.stringify({roleName: role}), { headers: DefaultHeaders });
  }

  getCompanySocialLinks(id: any): Observable<SuccessResponse<UpdateCompanySocialLinks>> {
    return this.http.get<SuccessResponse<UpdateCompanySocialLinks>>(`${this.baseUrl}/core/api/dao/${id}/social-media`, { headers: DefaultHeaders });
  }

  updateCompanySocialLinks(id: string, command:UpdateCompanySocialLinks): Observable<SuccessResponse<any>> {
    return this.http.put<SuccessResponse<string>>(`${this.baseUrl}/core/api/dao/${id}/social-media`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  storeFile(files: FileList, id: string): Observable<SuccessResponse<string>> {
    const file: File = files[0];
    const formData = new FormData();
    formData.append('file', file, file.name);
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/dao/${id}/documents/files`, formData, { headers: headers });
  }

  setUserSubscribedCompany(companyId: string, command: SetUserSubscribedCompanyCommand): Observable<SuccessResponse<any>> {
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/dao/${companyId}/newsletter`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  deleteUserSubscribedCompany(companyId: string, command: DeleteUserSubscribedCompanyCommand): Observable<SuccessResponse<any>> {
    return this.http.delete<SuccessResponse<any>>(`${this.baseUrl}/core/api/dao/${companyId}/newsletter`, { headers: DefaultHeaders });
  }
  
  addProposal(companyId: string, command: AddProposalCommand): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/dao/${companyId}/proposals`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  getProposals(companyId: string, take: number = 9, skip: number = 0): Observable<SuccessResponse<PaginationResponse<Proposal>>> {
    return this.http.get<SuccessResponse<PaginationResponse<Proposal>>>(`${this.baseUrl}/core/api/dao/${companyId}/proposals?take=${take}&skip=${skip}`, { headers: DefaultHeaders });
  }

  vote(companyId: string, proposalId: string, command: VoteCommand): Observable<SuccessResponse<any>> {
    return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/dao/${companyId}/proposals/${proposalId}`, JSON.stringify(command), { headers: DefaultHeaders });
  }
  
  getHolders(id: any, tokenId: string = '', skip: number = 0, take: number = 30): Observable<SuccessResponse<PaginationResponse<CompanyHolder>>> {
    return this.http.get<SuccessResponse<PaginationResponse<CompanyHolder>>>(`${this.baseUrl}/core/api/dao/${id}/holders?tokenId=${tokenId}&skip=${skip}&take=${take}`, { headers: DefaultHeaders });
  }

  createCompanyDocument(id: string, command: ModifyCompanyDocumentCommand): Observable<SuccessResponse<object>> {
    return this.http.post<SuccessResponse<object>>(`${this.baseUrl}/core/api/dao/${id}/documents`, JSON.stringify(command), { headers: DefaultHeaders })
  }

  // getCompanyDocumentTypes(): Observable<SuccessResponse<GetCompanyDocumentTypesResponse>> {
  //   return this.http.get<SuccessResponse<GetCompanyDocumentTypesResponse>>(`${this.baseUrl}/core/api/dao/documents/types`, { headers: DefaultHeaders });
  // }

  editCompanyDocumentContents(id: string, command: EditCompanyDocumentContentsCommand): Observable<SuccessResponse<object>> {
    return this.http.post<SuccessResponse<object>>(`${this.baseUrl}/core/api/dao/${id}/document`, JSON.stringify(command), { headers: DefaultHeaders })
  }
  uploadCompanyDocument(id: string, command: UploadCompanyDocumentCommand): Observable<SuccessResponse<string>> {
    const file: File = command.content[0];
    const formData = new FormData();
    formData.append('file', file);
    formData.append('language', command.language);
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/dao/${id}/document/upload`, formData, { headers: headers })
  }
  getExportedPdf(command: GetExportedPdfCommand): Observable<any> {
    return this.http.post(`https://pdf-converter.cke-cs.com/v1/convert`, JSON.stringify(command),
      { headers: PdfHeaders, responseType: "blob" });
  }


  public getCompanyDocuments(id: string): Observable<SuccessResponse<GetCompanyDocumentsResponse>> {
    return this.http.get<SuccessResponse<GetCompanyDocumentsResponse>>(`${this.baseUrl}/core/api/dao/${id}/getdocuments`, { headers: DefaultHeaders });
  }

  public getCompanyDocumentContent(id: string, type: string, language: string = 'en'): Observable<SuccessResponse<CompanyDocumentContent>> {
    return this.http.get<SuccessResponse<CompanyDocumentContent>>(`${this.baseUrl}/core/api/dao/${id}/documents/${type}?language=${language}`, { headers: DefaultHeaders });
  }


}

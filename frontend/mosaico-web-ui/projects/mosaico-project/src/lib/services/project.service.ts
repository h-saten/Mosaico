import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigService, SuccessResponse, DefaultHeaders, PatchModel, PaginationResponse, PdfHeaders } from 'mosaico-base';
import { CreateProjectCommand, DeleteUserSubscribedProjectCommand, EditProjectDocumentContentsCommand, GetExportedPdfCommand, GetTemplateContentsCommand, ModifyProjectDocumentCommand, SetUserSubscribedProjectCommand, UpdateProjectCommand, UpdateProjectStagesCommand, UpdateProjectVisibilityCommand, UploadProjectDocumentCommand, UpsertProjectStageCommand } from '../commands';
import { GetInvitationsResponse, GetProjectDocumentsResponse, GetProjectDocumentTemplateResponse, GetProjectDocumentTypesResponse, GetProjectInvestorsResponse, GetProjectResponse, GetProjectSaleDetailsResponse, GetProjectStagesResponse, GetTokenomicsResponse, ProjectPreValidationResponse, ProjectScoreResponse, SubscribePrivateSaleResponse, TransactionFeeResponse } from '../responses';
import { Stage, ProjectPermissions, TeamMember, ProjectsList, ProjectDocumentContent, Partner, ProjectSubscriber } from '../models';
import { PreValidationQuery } from '../queries';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private baseUrl = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getConfig().gatewayUrl;
  }

  createProject(command: CreateProjectCommand): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  updateDescription(projectId: string, description: string): Observable<SuccessResponse<void>> {
    return this.http.put<SuccessResponse<void>>(`${this.baseUrl}/core/api/projects/${projectId}/description`, JSON.stringify(description), { headers: DefaultHeaders });
  }

  getProject(id: string): Observable<SuccessResponse<GetProjectResponse>> {
    return this.http.get<SuccessResponse<GetProjectResponse>>(`${this.baseUrl}/core/api/projects/${id}`, { headers: DefaultHeaders });
  }

  submitForReview(id: string): Observable<SuccessResponse<boolean>> {
    return this.http.put<SuccessResponse<boolean>>(`${this.baseUrl}/core/api/projects/${id}/status/review`, null, { headers: DefaultHeaders });
  }

  getIsProjectVisited(id: string): Observable<SuccessResponse<boolean>>{
    return this.http.get<SuccessResponse<boolean>>(`${this.baseUrl}/core/api/projects/${id}/isprojectvisited`, { headers: DefaultHeaders });
  }

  updateProjectVisitor(id: string): Observable<SuccessResponse<string>>{
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${id}/updatevisitor`, { headers: DefaultHeaders });
  }

  updateStages(id: string, command: UpdateProjectStagesCommand): Observable<SuccessResponse<Stage[]>> {
    return this.http.post<SuccessResponse<Stage[]>>(`${this.baseUrl}/core/api/projects/${id}/stages`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  update(id: string, command: UpdateProjectCommand): Observable<SuccessResponse<object>> {
    return this.http.put<SuccessResponse<object>>(`${this.baseUrl}/core/api/projects/${id}`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  getProjectStages(id: string): Observable<SuccessResponse<GetProjectStagesResponse>> {
    return this.http.get<SuccessResponse<GetProjectStagesResponse>>(`${this.baseUrl}/core/api/projects/${id}/stages`, { headers: DefaultHeaders });
  }

  patchProject(id: string, expressions: PatchModel[]): Observable<SuccessResponse<any>> {
    return this.http.patch<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${id}`, JSON.stringify(expressions), { headers: DefaultHeaders });
  }

  createProjectDocument(id: string, command: ModifyProjectDocumentCommand): Observable<SuccessResponse<object>> {
    return this.http.post<SuccessResponse<object>>(`${this.baseUrl}/core/api/projects/${id}/documents`, JSON.stringify(command), { headers: DefaultHeaders })
  }

  getProjectPermissions(id: string | null, userId?: string): Observable<SuccessResponse<ProjectPermissions>> {
    if (userId && userId.length > 0) {
      return this.http.get<SuccessResponse<ProjectPermissions>>(`${this.baseUrl}/core/api/projects/${id}/permissions?userId=${userId}`, { headers: DefaultHeaders });
    }
    else {
      return this.http.get<SuccessResponse<ProjectPermissions>>(`${this.baseUrl}/core/api/projects/${id}/permissions`, { headers: DefaultHeaders });
    }
  }

  getProjectTeamMembers(id: any): Observable<SuccessResponse<PaginationResponse<TeamMember>>> {
    return this.http.get<SuccessResponse<PaginationResponse<TeamMember>>>(`${this.baseUrl}/core/api/pages/${id}/team-members`, { headers: DefaultHeaders });
  }

  addUpdateTeamMember(member: TeamMember): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${member.pageId}/team-member`, member, { headers: DefaultHeaders });
  }

  deleteTeamMember(id: string,memberId:string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/core/api/pages/${id}/team-member/${memberId}`, { headers: DefaultHeaders });
  }

  getProjectSaleReport(id: any): Observable<SuccessResponse<GetProjectSaleDetailsResponse>> {
    return this.http.get<SuccessResponse<GetProjectSaleDetailsResponse>>(`${this.baseUrl}/core/api/projects/${id}/sale/report`, { headers: DefaultHeaders });
  }

  uploadTeamMemberProfile(projectId: string, fileToUpload: File): Observable<SuccessResponse<string>> {
    const endpoint = `${this.baseUrl}/core/api/projects/${projectId}/documents/files`;
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.http.post<SuccessResponse<string>>(endpoint, formData);
  }


  //partners
  getProjectPartners(id: any): Observable<SuccessResponse<PaginationResponse<Partner>>> {
    return this.http.get<SuccessResponse<PaginationResponse<Partner>>>(`${this.baseUrl}/core/api/pages/${id}/partners`, { headers: DefaultHeaders });
  }
  addUpdatePartner(partner: Partner): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${partner.pageId}/partner`, partner, { headers: DefaultHeaders });
  }
  uploadPartnerProfile(projectId: string, fileToUpload: File): Observable<SuccessResponse<string>> {
    const endpoint = `${this.baseUrl}/core/api/projects/${projectId}/documents/files/partner`;
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.http.post<SuccessResponse<string>>(endpoint, formData);
  }
  deleteProjectPartner(id: string,partnerId:string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/core/api/pages/${id}/partner/${partnerId}`, { headers: DefaultHeaders });
  }



  preValidateProject(query: PreValidationQuery): Observable<SuccessResponse<ProjectPreValidationResponse>> {
    return this.http.post<SuccessResponse<ProjectPreValidationResponse>>(`${this.baseUrl}/core/api/projects/pre-validation`, JSON.stringify(query), { headers: DefaultHeaders });
  }

  getProjectDocumentTypes(): Observable<SuccessResponse<GetProjectDocumentTypesResponse>> {
    return this.http.get<SuccessResponse<GetProjectDocumentTypesResponse>>(`${this.baseUrl}/core/api/projects/documents/types`, { headers: DefaultHeaders });
  }

  getTemplateContent(command: GetTemplateContentsCommand): Observable<SuccessResponse<GetProjectDocumentTemplateResponse>> {
    return this.http.get<SuccessResponse<GetProjectDocumentTemplateResponse>>(`${this.baseUrl}/core/api/projects/templates/contents?language=${command.language}&key=${command.key}`, { headers: DefaultHeaders });
  }

  editProjectDocumentContents(id: string, command: EditProjectDocumentContentsCommand): Observable<SuccessResponse<object>> {
    return this.http.post<SuccessResponse<object>>(`${this.baseUrl}/core/api/projects/${id}/document`, JSON.stringify(command), { headers: DefaultHeaders })
  }
  uploadProjectDocument(id: string, command: UploadProjectDocumentCommand): Observable<SuccessResponse<string>> {
    const file: File = command.content[0];
    const formData = new FormData();
    formData.append('file', file);
    formData.append('language', command.language);
    formData.append('type', command.type);
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${id}/document/upload`, formData, { headers: headers })
  }
  getExportedPdf(command: GetExportedPdfCommand): Observable<any> {
    return this.http.post(`https://pdf-converter.cke-cs.com/v1/convert`, JSON.stringify(command),
      { headers: PdfHeaders, responseType: "blob" });
  }

  getCompanyProjects(skip: number = 0, take: number = 10, companyId: string): Observable<SuccessResponse<PaginationResponse<ProjectsList>>> {
    return this.http.get<SuccessResponse<PaginationResponse<ProjectsList>>>(`${this.baseUrl}/core/api/dao/${companyId}/projects?skip=${skip}&take=${take}`, { headers: DefaultHeaders });
  }

  updateProjectLogo(file: File, id: string): Observable<SuccessResponse<string>> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${id}/logo`, formData, { headers });
  }

  public getProjectDocuments(id: string): Observable<SuccessResponse<GetProjectDocumentsResponse>> {
    return this.http.get<SuccessResponse<GetProjectDocumentsResponse>>(`${this.baseUrl}/core/api/projects/${id}/documents`, { headers: DefaultHeaders });
  }

  public getProjectDocumentsByLanguage(id: string, language: string = 'en'): Observable<SuccessResponse<GetProjectDocumentsResponse>> {
    return this.http.get<SuccessResponse<GetProjectDocumentsResponse>>(`${this.baseUrl}/core/api/projects/${id}/documents?lang=${language}`, { headers: DefaultHeaders });
  }

  public getProjectDocumentContent(id: string, type: string, language: string = 'en'): Observable<SuccessResponse<ProjectDocumentContent>> {
    return this.http.get<SuccessResponse<ProjectDocumentContent>>(`${this.baseUrl}/core/api/projects/${id}/documents/${type}?language=${language}`, { headers: DefaultHeaders });
  }

  setUserSubscribesProject(projectId: string, command: SetUserSubscribedProjectCommand): Observable<SuccessResponse<any>> {
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${projectId}/newsletter`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  deleteUserSubscribesProject(projectId: string, command: DeleteUserSubscribedProjectCommand): Observable<SuccessResponse<any>> {
    return this.http.delete<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${projectId}/newsletter`, { headers: DefaultHeaders });
  }

  getProjectScore(id: string): Observable<SuccessResponse<ProjectScoreResponse>> {
    return this.http.get<SuccessResponse<ProjectScoreResponse>>(`${this.baseUrl}/core/api/projects/${id}/score`, { headers: DefaultHeaders });
  }

  getTokenomics(id: string): Observable<SuccessResponse<GetTokenomicsResponse>> {
    return this.http.get<SuccessResponse<GetTokenomicsResponse>>(`${this.baseUrl}/core/api/projects/${id}/tokenomics`, { headers: DefaultHeaders });
  }

  createStage(id: string, command: UpsertProjectStageCommand): Observable<SuccessResponse<string>> {
    return this.http.put<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${id}/stages`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  updateStage(id: string, stageId: string, command: UpsertProjectStageCommand): Observable<SuccessResponse<string>> {
    return this.http.put<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${id}/stages/${stageId}`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  deleteStage(id: string, stageId: string): Observable<SuccessResponse<any>> {
    return this.http.delete<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${id}/stages/${stageId}`, { headers: DefaultHeaders });
  }

  deployCrowdsale(id: string): Observable<SuccessResponse<any>> {
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${id}/crowdsale`, null, { headers: DefaultHeaders });
  }

  deployStage(id: string, stageId: string): Observable<SuccessResponse<any>>{
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${id}/stages/${stageId}/contract`, null, { headers: DefaultHeaders });
  }

  subscribePrivateSale(code: string): Observable<SuccessResponse<SubscribePrivateSaleResponse>> {
    return this.http.put<SuccessResponse<SubscribePrivateSaleResponse>>(`${this.baseUrl}/core/api/projects/stages`, JSON.stringify(code), { headers: DefaultHeaders });
  }

  getCode(id: string, stageId: string): Observable<SuccessResponse<string>>{
    return this.http.get<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${id}/stages/${stageId}/code`, { headers: DefaultHeaders });
  }

  getInvestors(id: string, skip: number = 0, take: number = 10, version: string = '1.0'): Observable<SuccessResponse<GetProjectInvestorsResponse>>{
    const requestData = { skip: skip, take: take, 'api-version': version };
    return this.http.get<SuccessResponse<GetProjectInvestorsResponse>>(`${this.baseUrl}/core/api/projects/${id}/wallet`, { headers: DefaultHeaders, params: requestData });
  }

  getFee(id: string): Observable<SuccessResponse<TransactionFeeResponse>> {
    return this.http.get<SuccessResponse<TransactionFeeResponse>>(`${this.baseUrl}/core/api/projects/${id}/fee`, { headers: DefaultHeaders })
  }

  like(id: string): Observable<SuccessResponse<any>>{
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${id}/like`, null, { headers: DefaultHeaders });
  }

  getSubscribers(id: string, skip: number = 0, take: number = 10): Observable<SuccessResponse<PaginationResponse<ProjectSubscriber>>> {
    return this.http.get<SuccessResponse<PaginationResponse<ProjectSubscriber>>>(`${this.baseUrl}/core/api/projects/${id}/subscribers`, 
      { headers: DefaultHeaders, params: {skip, take} });
  }

  public exportSubscribers(projectId: string): Observable<HttpResponse<Blob>> {
    return this.http.post(`${this.baseUrl}/core/api/projects/${projectId}/subscribers/export`, {}, { responseType: "blob", observe: 'response' });
  }

  public updatePublicity(id: string, visible: boolean): Observable<SuccessResponse<any>> {
    return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${id}/publicity`, JSON.stringify({isPublic: visible}), { headers: DefaultHeaders });
  }

  public updateProjectFee(id: string, feePercentage: number): Observable<SuccessResponse<any>> {
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${id}/settings/fee`, JSON.stringify({feePercentage}), { headers: DefaultHeaders });
  }

  public getProjectFee(id: string): Observable<SuccessResponse<number>> {
    return this.http.get<SuccessResponse<number>>(`${this.baseUrl}/core/api/projects/${id}/settings/fee`, { headers: DefaultHeaders });
  }
}

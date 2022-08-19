import { GetPackagesReponse } from './../responses/get-project-packages.response';
import { PaginationResponse, SuccessResponse, DefaultHeaders, ConfigService, PatchModel, PatchModelSocialLinks } from 'mosaico-base';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { CreateUpdateFaqCommand, CreateUpdateProjectArticleCommand, CreateUpdateProjectPackageCommand } from '../commands';
import { GetAboutPageResponse, GetFAQsReponse, GetNFTsResponse, GetIntroVideosResponse } from '../responses';
import { UpsertAboutPageCommand } from '..';
import { Page, ProjectArticle,IntroVideo, PageReview } from '../models';

@Injectable({
  providedIn: 'root'
})
export class TokenPageService {
  private baseUrl = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getConfig().gatewayUrl;
  }

  getPage(pageId: string, lang: string = ''): Observable<SuccessResponse<Page>> {
    return this.http.get<SuccessResponse<Page>>(`${this.baseUrl}/core/api/pages/${pageId}?language=${lang}`, { headers: DefaultHeaders });
  }

  patchPage(pageId: string, expressions: PatchModel[]): Observable<SuccessResponse<any>> {
    return this.http.patch<SuccessResponse<any>>(`${this.baseUrl}/core/api/pages/${pageId}`, JSON.stringify(expressions), { headers: DefaultHeaders });
  }

  patchPageSocialLinks(pageId: string, expressions: PatchModelSocialLinks[]): Observable<SuccessResponse<any>> {
    return this.http.patch<SuccessResponse<any>>(`${this.baseUrl}/core/api/pages/${pageId}`, JSON.stringify(expressions), { headers: DefaultHeaders });
  }

  // FAQ

  createFAQ(pageId: string, command: CreateUpdateFaqCommand): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${pageId}/faq`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  updateFAQ(pageId: string, faqId: string, command: CreateUpdateFaqCommand): Observable<SuccessResponse<string>> {
    return this.http.put<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${pageId}/faq/${faqId}`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  getFAQs(pageId: string, lang: string = ''): Observable<SuccessResponse<GetFAQsReponse>> {
    return this.http.get<SuccessResponse<GetFAQsReponse>>(`${this.baseUrl}/core/api/pages/${pageId}/faq?language=${lang}`, { headers: DefaultHeaders });
  }

  deleteFAQ(pageId: string, faqId: string): Observable<SuccessResponse<any>> {
    return this.http.delete<SuccessResponse<any>>(`${this.baseUrl}/core/api/pages/${pageId}/faq/${faqId}`, { headers: DefaultHeaders })
  }

  // end FAQ

  // NFT

  getNft(project_id: string, lang: string = ''): Observable<SuccessResponse<GetNFTsResponse>>  {
    return this.http.get<SuccessResponse<GetNFTsResponse>>(`${this.baseUrl}/core/api/nfts?projectId=${project_id}`)
  }

  // packages

  createPackage(pageId: string, command: CreateUpdateProjectPackageCommand): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${pageId}/packages`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  updatePackage(pageId: string, packageId: string, command: CreateUpdateProjectPackageCommand): Observable<SuccessResponse<string>> {
    return this.http.put<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${pageId}/packages/${packageId}`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  deletePackage(pageId: string, packageId: string): Observable<SuccessResponse<any>> {
    return this.http.delete<SuccessResponse<any>>(`${this.baseUrl}/core/api/pages/${pageId}/packages/${packageId}`, { headers: DefaultHeaders })
  }

  getPackages(pageId: string, lang: string = ''): Observable<SuccessResponse<GetPackagesReponse>> {
    return this.http.get<SuccessResponse<GetPackagesReponse>>(`${this.baseUrl}/core/api/pages/${pageId}/packages?language=${lang}`, { headers: DefaultHeaders });
  }

  updatePackageLogo(pageId: string, packageId: string, fileToUpload: File): Observable<SuccessResponse<string>> {
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${pageId}/packages/${packageId}/logo`, formData, { headers });
  }

  deletePackageLogo(pageId: string, packageId: string): Observable<SuccessResponse<any>> {
    return this.http.delete<SuccessResponse<any>>(`${this.baseUrl}/core/api/pages/${pageId}/packages/${packageId}/logo`, { headers: DefaultHeaders });
  }

  // end Packages

  // ABOUT
  getAboutPage(id: string, lang: string = ''): Observable<SuccessResponse<GetAboutPageResponse>> {
    return this.http.get<SuccessResponse<GetAboutPageResponse>>(`${this.baseUrl}/core/api/pages/${id}/about?language=${lang}`, { headers: DefaultHeaders });
  }
  upsertAboutPage(id: string, command: UpsertAboutPageCommand): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${id}/about`, JSON.stringify(command), { headers: DefaultHeaders });
  }
  //end ABOUT

  //News
  getArticles(skip:number, take:number, id: string): Observable<SuccessResponse<PaginationResponse<ProjectArticle>>> {
    return this.http.get<SuccessResponse<PaginationResponse<ProjectArticle>>>(`${this.baseUrl}/core/api/pages/${id}/articles?skip=${skip}&take=${take}`, { headers: DefaultHeaders });
  }
  upsertArticle(id: string, command: CreateUpdateProjectArticleCommand): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${id}/article`, JSON.stringify(command), { headers: DefaultHeaders });
  }
  deleteArticle(id: string): Observable<SuccessResponse<any>> {
    return this.http.delete<SuccessResponse<any>>(`${this.baseUrl}/core/api/pages/${id}/article`, { headers: DefaultHeaders });
  }
  hideArticle(id: string): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${id}/article/hide`, { headers: DefaultHeaders });
  }
  displayArticle(id: string): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${id}/article/display`, { headers: DefaultHeaders });
  }
  updateArticleCover(file: File, id: string): Observable<SuccessResponse<string>> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${id}/article/cover`, formData, { headers });
  }
  updateArticlePhoto(file: File, id: string): Observable<SuccessResponse<string>> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${id}/article/photo`, formData, { headers });
  }
  // logo

  uploadPageCover(pageId: string, fileToUpload: File, lang: string = 'en'): Observable<SuccessResponse<string>> {
    const endpoint = `${this.baseUrl}/core/api/pages/${pageId}/cover?language=${lang}`;
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.http.post<SuccessResponse<string>>(endpoint, formData);
  }

  deletePageCover(pageId: string, lang: string = 'en'): Observable<SuccessResponse<any>> {
    return this.http.delete<SuccessResponse<any>>(`${this.baseUrl}/core/api/pages/${pageId}/cover?language=${lang}`, { headers: DefaultHeaders });
  }

  getIntroVideos(id: string): Observable<SuccessResponse<GetIntroVideosResponse>>{
    return this.http.get<SuccessResponse<GetIntroVideosResponse>>(`${this.baseUrl}/core/api/pages/${id}/introvideos`, { headers: DefaultHeaders });
  }

  uploadPageIntroVideo(file: File,videoExternalLink:string, showLocalVideo: string, id: string){
    const formData = new FormData();
    formData.append('file', file, file.name);    
    formData.append('showLocalVideo', showLocalVideo);
    formData.append('videoExternalLink', videoExternalLink);
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${id}/introvideo/upload`, formData, { headers });
  }

  updatePageIntroVideoUrl(videoUrl: string,showLocalVideo: string, id: string){
    const formData = new FormData();
    formData.append('videoExternalLink', videoUrl);
    formData.append('showLocalVideo', showLocalVideo);
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/pages/${id}/introvideourl/update`, formData, { headers });
  }

  getPageReviews(id: string): Observable<SuccessResponse<PageReview[]>> {
    return this.http.get<SuccessResponse<PageReview[]>>(`${this.baseUrl}/core/api/pages/${id}/reviews`, { headers: DefaultHeaders });
  }
}

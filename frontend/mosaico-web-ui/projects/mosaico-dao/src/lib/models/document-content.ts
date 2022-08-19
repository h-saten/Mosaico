import { CompanyDocumentType } from './company-document-type';
export interface CompanyDocumentContent {
    id: string;
    companyId: string;
    title:string;    
    language: string;
    content: string;
}
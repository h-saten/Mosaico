import { CompanyDocumentType } from "./company-document-type";

export interface CompanyDocument {
  id: string;
  companyId: string;
  title:string;
  url: string;
  language: string;
};

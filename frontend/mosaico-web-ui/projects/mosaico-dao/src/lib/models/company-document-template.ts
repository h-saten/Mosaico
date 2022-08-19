import { CompanyDocumentType } from ".";

export interface CompanyDocumentTemplate {
  content: string;
  key: string;
  templateVersion: string;
  language: string;
  isEnabled: boolean;
  documentId: string;
  id: string;
  title: string;
};
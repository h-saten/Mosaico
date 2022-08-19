import { ProjectDocumentType } from ".";

export interface ProjectDocumentTemplate {
  content: string;
  key: string;
  templateVersion: string;
  language: string;
  isEnabled: boolean;
  documentId: string;
  id: string;
  title: string;
};

import { ProjectDocumentType } from "./project-document-type";

export interface ProjectDocument {
  id: string;
  projectId: string;
  url: string;
  type: ProjectDocumentType;
  language: string;
};

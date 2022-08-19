import { ProjectDocumentType } from './project-document-type';
export interface ProjectDocumentContent {
    id: string;
    projectId: string;
    type: ProjectDocumentType;
    language: string;
    content: string;
}
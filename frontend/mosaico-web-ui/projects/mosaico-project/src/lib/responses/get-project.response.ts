import { Project } from '../models';

export interface GetProjectResponse {
    project: Project;
    isSubscribed: boolean;
}
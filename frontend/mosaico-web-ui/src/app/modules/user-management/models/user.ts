import { UserInformation } from './user-information';

export interface User extends UserInformation{
    isAuthorized: boolean;
    permissions?: any;
};
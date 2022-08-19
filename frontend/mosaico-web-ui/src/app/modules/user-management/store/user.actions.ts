import { createAction, props } from '@ngrx/store';
import { GetUserProfilePermissionsResponse, UserInformation } from '../models';

export const setUserInformation = createAction('[User] Set User Information', props<UserInformation>());
export const setAccountVerified = createAction('[User] Set KYC completed', props<{ completed: boolean }>());
export const setAuthorized = createAction('[User] Set Authorized', props<{ isAuthorized: boolean }>());
export const setUserProfilePermissions = createAction('[User] Set user profile permissions', props<{perm: GetUserProfilePermissionsResponse}>());
export const cleanUserProfilePermissions = createAction('[User] Clear user profile permissions');
export const setEvaluationCompleted = createAction('[User] Complete evaluation', props<{completed: boolean}>());
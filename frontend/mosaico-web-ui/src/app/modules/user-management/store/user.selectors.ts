import { createFeatureSelector, createSelector } from "@ngrx/store";
import { User, UserInformation } from "../models";

export const featureKey = 'user';

export const selectUser = createFeatureSelector<User>(featureKey);

export const selectIsAuthorized = createSelector(
    selectUser,
    (state: User) => state.isAuthorized
);

export const selectUserInformation = createSelector(
    selectUser,
    (state: User) => ({
        email: state.email,
        firstName: state.firstName,
        id: state.id,
        lastName: state.lastName,
        isEmailVerified: state.isEmailVerified,
        username: state.username,
        isAMLVerified: state.isAMLVerified,
        isPhoneVerified: state.isPhoneVerified,
        phoneNumber: state.phoneNumber,
        photoUrl: state.photoUrl,
        country: state.country,
        timezone: state.timezone,
        postalCode: state.postalCode,
        city: state.city,
        street: state.street,
        dob: state.dob,
        hasKangaAccount: state.hasKangaAccount,
        evaluationCompleted: state.evaluationCompleted
    } as UserInformation)
);

export const selectUserIsVerified = createSelector(
    selectUser,
    (state: User) => state?.isAMLVerified
);

export const selectUserPermissions = createSelector(
    selectUser,
    (state: User) => state?.permissions
);

export const selectUserPhotoUrl = createSelector(
    selectUser,
    (state: User) => state?.photoUrl
)

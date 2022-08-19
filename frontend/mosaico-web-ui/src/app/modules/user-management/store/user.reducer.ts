import { createReducer, on } from "@ngrx/store";
import { User } from "../models";
import { setAuthorized, setAccountVerified, setUserInformation, setUserProfilePermissions, cleanUserProfilePermissions, setEvaluationCompleted } from './user.actions';

export const initialUser: User = {
  id: null,
  email: null,
  firstName: null,
  isAMLVerified: false,
  lastName: null,
  isAuthorized: false,
  isEmailVerified: false,
  username: null,
  photoUrl: null,
  isPhoneVerified: false,
  phoneNumber: null,
  isDeactivated: false,
  lastLogin: null,
  isAdmin: false,
  country: null,
  timezone: null,
  postalCode: null,
  city: null,
  street: null,
  dob: undefined,
  permissions: null,
  hasKangaAccount: false,
  evaluationCompleted: false
};

export const userReducers = createReducer(
  initialUser,
  on(setUserInformation, (state, info) => {
    return {
      ...state,
      ...info
    };
  }),
  on(setAccountVerified, (state, { completed }) => {
    return { ...state, isAMLVerified: completed };
  }),
  on(setAuthorized, (state, { isAuthorized }) => {
    return { ...state, isAuthorized };
  }),
  on(setUserProfilePermissions, (state, {perm}) => {
      return {...state, permissions: perm};
  }),
  on(cleanUserProfilePermissions, (state) => {
    return { ...state, permissions: null};
  }),
  on(setEvaluationCompleted, (state, {completed}) => {
    return {...state, evaluationCompleted: completed };
  })
);

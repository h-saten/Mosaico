import { User } from './../modules/user-management/models';
import { userReducers } from '../modules/user-management/store/user.reducer';
import {walletReducers} from "../modules/wallet";
import { globalReducers } from './reducers';

export interface AppState {
    user: User;
}

export const appReducers = {
    user: userReducers,
    wallet: walletReducers,
    global: globalReducers
}

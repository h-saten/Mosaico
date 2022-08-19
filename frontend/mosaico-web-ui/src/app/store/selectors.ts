import { createFeatureSelector, createSelector } from "@ngrx/store";
import { GlobalState } from "./reducers";

export const featureKey = 'global';
export const selectGlobal = createFeatureSelector<GlobalState>(featureKey);

export const selectBlockchain = createSelector(
    selectGlobal,
    (state: GlobalState) => state.selectedBlockchain
);

export const selectCurrentActiveBlockchains = createSelector(
    selectGlobal,
    (state: GlobalState) => state.activeBlockchains
);

export const selectCurrentUrl = createSelector(
    selectGlobal,
    (state: GlobalState) => state.selectedCurrentUrl
);

export const selectShowFullWidth = createSelector(
    selectGlobal,
    (state: GlobalState) => state.showFullWidth
);

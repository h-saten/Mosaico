import { NgbModalOptions } from "@ng-bootstrap/ng-bootstrap";

export const DEFAULT_MODAL_OPTIONS: NgbModalOptions = {
    size: 'lg',
    centered: true,
    backdrop: 'static',
    animation: true,
    modalDialogClass: 'modalDialogClass'
};

export enum USER_PERMISSIONS {
    "CAN_EDIT_PASSWORD" = "CAN_EDIT_PASSWORD",
    "CAN_READ" = "CAN_READ",
    "CAN_EDIT_EMAIL" = "CAN_EDIT_EMAIL",
    "CAN_EDIT_PROFILE" = "CAN_EDIT_PROFILE",
    "CAN_EDIT_PHONE" = "CAN_EDIT_PHONE",
    "CAN_VERIFY_ACCOUNT" = "CAN_VERIFY_ACCOUNT"
};

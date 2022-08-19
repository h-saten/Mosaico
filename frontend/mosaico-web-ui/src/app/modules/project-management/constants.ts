export enum PERMISSIONS {
    "CAN_READ" = "CAN_READ",
    "CAN_PURCHASE" = "CAN_PURCHASE",
    "CAN_EDIT_DETAILS" = "CAN_EDIT_DETAILS",
    "CAN_EDIT_STAKING" = "CAN_EDIT_STAKING",
    "CAN_EDIT_STAGES" = "CAN_EDIT_STAGES",
    "CAN_EDIT_VESTING" = "CAN_EDIT_VESTING",
    "CAN_EDIT_DOCUMENTS" = "CAN_EDIT_DOCUMENTS",
    "CAN_VIEW_DASHBOARD" = "CAN_VIEW_DASHBOARD"
};

export enum PROJECT_ROLES {
    "OWNER" = 'Owner',
    "MEMBER" = 'Member'
};

export enum ProjectPathEnum {
  Main = '', // overwiew
  About = 'about', // About Project
  Packages = 'packages', // Investment packages
  News = 'news', // Press room
  Faq = 'faq',
  Settings = 'settings',
  Overview = 'overview',
  Stats = 'stats',
  Company = 'company',
  Fund = 'buy',
  Edit = 'edit',
  PaymentConfirmed = 'orderConfirmation',
  NFTs = 'nfts'
}

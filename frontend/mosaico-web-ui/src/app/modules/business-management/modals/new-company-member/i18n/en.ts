export const locale = {
    lang: 'en',
    data: {
        COMPANY_OVERVIEW: {
            TOKEN_LIST_TITLE: "List of Tokens",
            NO_TOKENS: 'There are no tokens',
            NO_PROJECTS: 'There are no projects',
            ACTIONS: {
                CREATE: 'Create token',
                SUBSCRIBE: 'Subscribe'
            }
        },
        COMPANY_PROJECTS: {
            TITLE: 'Our projects',
            ACTIONS: {
                CREATE: 'Create project',
                DETAILS: 'Check project'
            }
        },
        COMPANY_PAGE_MENU: {
            OVERVIEW: "Overview",
            VOTING: "Voting",
            HOLDERS: "Hodlers",
            MEMBERS: "Members",
            WALLET: "Wallet",
            SETTINGS: "Settings",
            SOON: 'Soon'
        },
        COMPANY_INFO_CARD: {
            CONTACTS_TITLE: "Contacts",
            OFFICE_TITLE: "Our office",
            SOCIAL_TITLE: "Find us online",
            TAX_ID: "VAT ID"
        },
        COMPANY_SETTINGS_TABS: {
            DETAILS: 'Details',
            MEMBERS: 'DAO Members',
            KYB: 'Verification'
        },
        COMPANY_EDIT: {
            FORM: {
                NAME: {
                    LABEL: 'Company name',
                    PLACEHOLDER: 'Enter company name'
                },
                COUNTRY: {
                    LABEL: 'Country',
                    PLACEHOLDER: 'Select a country'
                },
                STREET: {
                    LABEL: 'Street',
                    PLACEHOLDER: 'Enter address of your registration'
                },
                POSTAL_CODE: {
                    LABEL: 'Postal code',
                    PLACEHOLDER: 'Enter postal code'
                },
                SIZE: {
                    LABEL: 'Company size',
                    PLACEHOLDER: 'Choose the size of your company',
                    OPTIONS: {
                        SMALL: 'Small (1-49)',
                        MEDIUM: 'Medium (50-249)',
                        LARGE: 'Large (250+)'
                    }
                },
                VAT: {
                    LABEL: "Company ID (VAT)",
                    PLACEHOLDER: "Enter your company's valid ID"
                },
                PHONE: {
                    LABEL: "Phone number",
                    PLACEHOLDER: "Enter the phone number"
                },
                EMAIL: {
                    LABEL: "Email",
                    PLACEHOLDER: "Enter email address"
                }
            },
            CONTACT_INFO_TITLE: "Contact information",
            MESSAGES: {
                INVALID_FORM: 'Values you entered are not valid. Please,try again',
                SUCCESS: 'Company was saved successfully',
                LEFT: 'Successfully left the company'
            },
            LEAVE_COMPANY_MODAL: {
                TITLE: 'Confirm your decision',
                MESSAGE: 'Are you sure you want to leave this DAO?'
            },
            ACTIONS: {
                LEAVE: 'Leave company',
                SAVE: 'Save'
            }
        },
        COMPANIES: {
            MY: {
                TITLE: "DAO",
                NO_COMPANIES: "You are not part of any DAO",
                ACTIONS: {
                    DETAILS: 'View',
                    WALLET: 'Wallet',
                    CREATE: 'Create DAO'
                }
            }
        },
        COMPANY_WALLET: {
            SUMMARY: {
                TOTAL: 'Total assets',
                COPY_HINT: 'Copy to clipboard',
                ACTIONS: {
                    DEPOSIT: 'Deposit',
                    SEND: 'Send',
                    WITHDRAW: 'Withdraw'
                },
                MESSAGES: {
                    COPIED: 'Copied!'
                }
            },
            STATS: {
                COMING_SOON: 'We are going to launch statistics soon',
                GET_NOTIF: "Get notified"
            },
            ASSETS: {
                TITLE: "Assets",
                NO_ASSETS: "There are no assets. Time to change it...",
                ACTIONS: {
                    EXCHANGE: 'Exchange',
                    STAKE: 'Stake',
                    MANAGE: 'Manage'
                }
            },
            TRANSACTIONS: {
                TITLE: "Transactions",
                TABLE: {
                    TOKEN: 'Token',
                    HASH: 'Transaction',
                    SOURCE: 'Source',
                    AMOUNT: 'Amount',
                    DESTINATION: 'Destination'
                },
                NO_TRANSACTIONS: 'There are no transactions',
                ACTIONS: {
                    LOAD_MORE: 'View more'
                }
            }
        },
        MODALS: {
            WALLET_SEND: {
                TITLE: 'Send funds',
                ACTIONS: {
                    SEND: 'Send',
                    CANCEL: 'Cancel'
                },
                FORM: {
                    TOKEN: {
                        LABEL: 'Token',
                        PLACEHOLDER: "e.g USDT",
                        INVALID :'Invalid token'
                    },
                    ADDRESS: {
                        LABEL: 'Destination address',
                        PLACEHOLDER: 'e.g 0x71C7656EC7ab88b098defB751B7401B5f6d8976F',
                        INVALID: 'Invalid address'
                    },
                    AMOUNT: {
                        LABEL: 'Amount',
                        PLACEHOLDER: 'e.g 25.00',
                        INVALID: 'Invalid amount'
                    },
                    CURRENT_BALANCE: 'Available'
                },
                MESSAGES: {
                    INVALID_FORM: 'Form contains invalid values',
                    SUCCESS: 'Your transaction will be processed shortly'
                }
            }
        }
    }
};

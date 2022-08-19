export const locale = {
    lang: 'en',
    data: {
        PUBLIC_COMPANIES:{
          HEADER_TITLE:"Companies who <br><span class='title-mosaico'>trusted us</span>",
          HEADER_DESC:"A decentralized autonomous organization,  represented by rules encoded as a computer program that is transparent, controlled by the organization members.",
          TRUESTED_COMPANIES:"Trusted companies",
          SEARCH:"Search",
          COMPANY_DESC:"Company description",
          VIEW_DAO:"View DAO Page",
          FOLLOW:"Follow DAO",
          UNFOLLOW:"Unfollow",
          VIEW_MORE:"View more DAO Pages",
          LOADING:"Loading...",
          BTN:{
            BUILD_DAO:"Build your DAO"
          },
          // NO_COMPANIES: "There are no companies currently",
          NO_COMPANIES: "No results",
          OPEN_POLLS: "Open",
          TOTAL_POLLS: "Total"
        },
        KYB: {
            KNOW_YOUR_BUSINESS: 'Know Your Business',
            PLEASE_UPLOAD_REGISTRATION: 'Please upload a scan of company registration',
            PLEASE_UPLOAD_ADDRESS: 'Please upload a scan of company address',
            COMPANY_REGISTRATION: 'Company registration',
            DOWNLOAD_COMPANY_REGISTRATION: 'Download company registration',
            COMPANY_ADDRESS: 'Company address',
            DOWNLOAD_COMPANY_ADDRESS: 'Download company address',
            SHAREHOLDERS: 'Shareholders',
            FULL_NAME: 'Full name',
            EMAIL: 'Email',
            PERCENT_SHARE: 'Percent share',
            ADD_SHAREHOLDER: 'Add Shareholder',
            SUBMIT_DATA: 'Submit data'

        },

        COMPANY_OVERVIEW: {
            TOKEN_LIST_TITLE: "List of Tokens",
            NO_TOKENS: 'There are no tokens',
            NO_DOCUMENTS: 'There are no documents',
            NO_PROJECTS: 'There are no projects',
            ACTIONS: {
                CREATE: 'Create token',
                IMPORT: 'Import token',
                SUBSCRIBE: 'Follow us',
                UNSUBSCRIBE:"Unfollow"
            }
        },
        COMAPNY_DOCUMENT:{
            TITLE:"Documents",
            LANGUAGE:{
                TITLE:"Select Language"
            },
            ACTIONS:{
                ADD_FILES: "Add document"
            }
        },
        COMPANY_DESCRIPTION:{
            TITLE:'Company Description'
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
            SOCIAL:'Social Media',
            MEMBERS: 'DAO Members',
            KYB: 'Verification'
        },
        SOCIAL_LINKS:{
          CARD:{
            TITLE:'Social media for your DAO',
            TELEGRAM: 'Telegram Channel',
            YOUTUBE: 'Youtube Channel',
            LINKEDIN: 'LinkedIn Profile',
            FACEBOOK: 'Facebook Page',
            TWITTER: 'Twitter Profile',
            INSTAGRAM: 'Instagram Profile',
            MEDIUM: 'Medium'
          },
          MESSAGE:{
            SUCCESS:'Social media liks has been updated.',
            FAILED:'Data not saved! Please check and try again.',
            INVALID_URL:'You entered invalid URL (URL must start with https)'
          }
        },
        MEMBERS: {
            INVITATIONS: 'Invitations',
            STATUS: 'Status',
            ROLE: 'Role',
            EXPIRATION: 'Expiration',
            ACTIONS: 'Actions'
        },
        COMPANY_EDIT: {
            CONTACT_INFO_TITLE: 'Contact information',
            FORM: {
                NAME: {
                    LABEL: 'DAO name',
                    PLACEHOLDER: 'Enter DAO name',
                    ERROR: 'Name is required'
                },
                DESCRIPTION:{
                    LABEL: 'Company description',
                    PLACEHOLDER: 'Enter Company description',
                },
                COUNTRY: {
                    LABEL: 'Country',
                    PLACEHOLDER: 'Select a country'
                },
                NETWORK: {
                    LABEL: 'Blockchain network',
                    ERROR: 'Network is required',
                    PLACEHOLDER: "Choose your blockchain"
                },
                STREET: {
                    LABEL: 'Street',
                    PLACEHOLDER: 'Enter address of your registration'
                },
                POSTAL_CODE: {
                    LABEL: 'Postal code',
                    PLACEHOLDER: 'Enter postal code'
                },
                REGION: {
                    LABEL: "State / Province",
                    PLACEHOLDER: "Enter region/province/state"
                },
                ACCEPT_REGULATION: {
                    LABEL: "I accept regulations and unified contract of DAO."
                },
                SIZE: {
                    LABEL: 'Organization size',
                    PLACEHOLDER: 'Choose the size of your company',
                    OPTIONS: {
                        SMALL: 'Small (1-49)',
                        MEDIUM: 'Medium (50-249)',
                        LARGE: 'Large (250+)'
                    }
                },
                VAT: {
                    LABEL: "VAT ID",
                    PLACEHOLDER: "Enter company's ID"
                },
                PHONE: {
                    LABEL: "Phone number",
                    PLACEHOLDER: "Enter the phone number"
                },
                EMAIL: {
                    LABEL: "Email",
                    PLACEHOLDER: "Enter email address"
                },
                LEGAL_SECTION: 'Company Information',
                SETTINGS_SECTION: 'Settings',
                VOTING: {
                    LABEL: 'Enable voting'
                },
                EVERYBODY_CREATES: {
                    LABEL: 'Everybody can create a vote'
                },
                PERIOD: {
                    OPTIONS: {
                        DAY: "Day",
                        MONTH: "Month",
                        WEEK: "Week"
                    }
                },
                QUORUM: {
                    LABEL: 'Quorum',
                    PLACEHOLDER: 'Enter numeric value',
                    HINT: 'Quorum is a percentage (%) of tokens that should participate in the vote so it becomes valid'
                },
                INITIAL_VOTING_DELAY: {
                    LABEL: "Voting delay",
                    PLACEHOLDER: "Select initial delay",
                    HINT: "How long it takes to activate proposal after its creation"
                },
                INITIAL_VOTING_PERIOD: {
                    LABEL: "Voting period",
                    PLACEHOLDER: "Select voting period",
                    HINT: "How long it is possible to vote after proposal is activated"
                }
            },
            TITLE: "Create DAO",
            MESSAGES: {
                INVALID_FORM: 'Values you entered are not valid. Please,try again',
                SUCCESS: 'DAO was saved successfully',
                LEFT: 'Successfully left the DAO',
                INITIATED_TRANSACTION: "Transaction was initiated. Please, wait..."
            },
            LEAVE_COMPANY_MODAL: {
                TITLE: 'Confirm your decision',
                MESSAGE: 'Are you sure you want to leave this DAO?'
            },
            ACTIONS: {
                LEAVE: 'Leave DAO',
                SAVE: 'Save',
                SAVING: 'Saving...',
                CANCEL: 'Cancel',
                UPDATE: 'Save'
            }
        },
        COMPANIES: {
            MY: {
                TITLE: "DAO",
                NO_COMPANIES: "You are not part of any DAO",
                DESCRIPTION: "You can create your own DAO, which is a decentralized enterprise. DAO (interchangeably DAC) - A new format of an organization (e.g., a company) that, through decentralization, is transparent (public) as well as democratized, and, through tokenization, enables capital to be raised for its development, traded on the secondary market, and managed by the company through voting and investor relations. The DAO becomes the entity responsible for the collection, its management can be revoked, and the funds raised are available for inspection by Tokenaries.",
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
                NETWORK: {
                    PLACEHOLDER: "Choose the network"
                },
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
        SUBSCRIPTION_ALREADY_EXISTS:"You have already subcribed to this DAO newsletter",
        COMPANY_VOTING: {
            TITLE: 'Open votings',
            ACTIONS: {
                CREATE: "Add proposal"
            },
            NO_PROPOSALS: "Currently there are no proposals",
            CREATED_BY: "Created by",
            FOR: "Yes",
            AGAINST: "No",
            MESSAGES: {
                COPIED: "Copied",
                VOTE_SUCCESS: 'Vote was saved'
            },
            TIME_LEFT: "Closes at",
            STARTS_AT: 'Starts at',
            CLOSED: 'Closed',
            TOTAL_VOTED: 'People voted'
        },
        MODALS: {
            SUBSCRIPTION_TO_NEWSLETTER:{
              TITLE : "Sign up for company alert",
              MESSAGES: {
                INFO: "Do you want to learn more about the company? To be one of the first investors? Sign up for our whitelist!",
                INFO_FOR_NOT_LOGGED_IN: "For logged in only for now!",
                INFO_FOR_NOT_LOGGED_IN2: "Log in",
                INFO_ALREADY_SUBSCRIBED: "You are already subscribed to the company newsletter.",
                INFO_SUBSCRIBED_TO_NEWSLETTER_LOGGED_IN: "Subscribed to the newsletter!",
                INFO_SUBSCRIBED_TO_NEWSLETTER_NOT_LOGGED_IN: "Receive a message from us and click on the link to confirm that you want to subscribe to the newsletter.",
                INFO_UNSUBSCRIBED_TO_NEWSLETTER: "Unsubscribed from the newsletter",
                INFO_CONFIRM_UNSUBSCRIBED: "Do you want to unsubscribe from the newsletter?",
              },
              ACTIONS: {
                SAVE: "Sign up",
                CANCEL: 'Cancel',
                SIGN_ME_OUT: 'Sign me out',
              },
            },
            NEW_TOKEN: {
                TITLE: 'Create token'
            },
            IMPORT_TOKEN: {
              TITLE: 'Import token'
            },
            COMPANY_LOGO_EDITOR: {
                TITLE: "Edit company logo",
                ACTIONS: {
                    SAVE: "Save",
                    CANCEL: 'Cancel',
                    ADD: 'Add logo',
                    CHANGE: 'Edit logo'
                },
                MESSAGES: {
                    SUCCESS: "Logo was successfully updated",
                    INVALID_FORM: "There is invalid data. Cannot save"
                }
            },
            WALLET_SEND: {
                TITLE: 'Send',
                ACTIONS: {
                    SEND: 'Send',
                    CANCEL: 'Cancel',
                    CLOSE: 'Done'
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
                    SUCCESS: 'Your transaction will be processed shortly',
                    TRANSACTION_INITIATED: "Transaction was initiated. Please wait"
                }
            },
            NEW_COMPANY: {
                TITLE: 'New DAO'
            },
            NEW_PROPOSAL: {
                MESSAGES: {
                    SUCCESS: "Proposal was successfully created",
                    INVALID_FORM: "Form contains invalid values",
                    TRANSACTION_INITIATED: "Transaction was initiated. Please wait"
                },
                TITLE: "New proposal",
                ACTIONS: {
                    CANCEL: "Cancel",
                    CREATE: "Create"
                },
                FORM: {
                    TITLE: {
                        LABEL: "Title",
                        ERROR: "Title is invalid",
                        PLACEHOLDER: "Enter the short question"
                    },
                    DESCRIPTION: {
                        LABEL: "Description",
                        ERROR: "Invalid description",
                        PLACEHOLDER: "Tell more about your question"
                    },
                    QUORUM: {
                        LABEL: "Quorum Threshold",
                        ERROR: "Invalid threshold",
                        PLACEHOLDER: "Enter how many % of token hodlers should vote"
                    },
                    TOKEN: {
                        LABEL: "Token",
                        ERROR: "Invalid token",
                        PLACEHOLDER: "Choose the token the proposal is related to"
                    }
                }
            }
        },
        HOLDERS_TAB: {
            NO_HODLERS: "There are no hodlers",
            BALANCE: "Balance",
            WALLET_ADDRESS: "Wallet address",
            FILTER_BY: "Filter by:"
        },
        INSUFFICIENT_FUNDS: "Insufficient balance"
    }
};

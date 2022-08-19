export const locale = {
    lang: 'en',
    data: {
        HEADER: {
            BUTTON_FOLLOW_US: "Follow us",
            BUTTON_NO_FOLLOW_US: "Unfollow",
            BUTTON_SHARE: "Share"
        },
        FOOTER: {
            CONTACTS: "Contact",
            DESCRIPTION: "Our Company",
        },
        MESSAGE: {
            SUCCESS: "Success!",
            PARTNER_DELETED: "Partner deleted successfully!"
        },
        NEW_PROJECT: {
            TITLE: "Create a project",
            FORM: {
                TITLE: {
                    LABEL: "Project name",
                    PLACEHOLDER: "Name your project",
                    ERROR: "Invalid project name",
                    ERROR_DUPLICATE: 'Such project already exists'
                },
                SLUG: {
                    LABEL: "Slug",
                    PLACEHOLDER: "Enter slug",
                    ERROR: "Invalid slug",
                    ERROR_DUPLICATE: 'Choose another slug'
                },
                COMPANY: {
                    LABEL: 'DAO',
                    PLACEHOLDER: 'Select DAO',
                    ERROR: 'DAO is required'
                },
                DESCRIPTION: {
                    LABEL: "Short project description",
                    PLACEHOLDER: "Describe shortly what your project is about",
                    ERROR: "Enter valid description"
                },
                ACTIONS: {
                    CONTINUE: "Continue",
                    CREATE_DAO: 'Create DAO'
                }
            },
            MESSAGES: {
                INVALID_FORM: 'Form has invalid values. Please, fix them to continue',
                SUCCESSFULLY_CREATED: 'Project successfully created',
                NO_DAO: 'You are not part of any DAO. Create one first!'
            }
        },
        PROJECT: {
            'counter.days': "Days",
            'counter.hours': "Hours",
            'counter.minutes': "Minutes",
            'counter.seconds': "Seconds",
            'info.col.start': 'Start',
            'info.col.end': 'End',
            'info.col.blockchain': 'Blockchain',
            'info.col.token_price': 'Token price',
            'bar.title': 'Funds raised',
            'bar.col.1': 'Investors',
            'bar.col.2': 'Sold tokens',
            'bar.col.3': 'Soft cap',
            'bar.col.4': 'Hard cap',
            'bar.tooltip.softcap': `Softcap refers to the minimum defined limit for the collection of funds specified by a project's team for its fund-raising`,
            'menu.overview': 'Overview',
            'menu.about': 'About project',
            'menu.packages': 'Investment packages',
            'menu.news': 'Press room',
            'menu.faq': 'FAQ',
            'menu.feedback': "Investors' opinions",
            "faq.title": "Frequently asked questions",
            "feedback.title": "Investors' opinions",
            // "faq.title" : "FAQ",
            "packages.title": 'Choose your package',
            "packages.btn.select": "Select plan",
            "show_more.name.articles": "articles",
            "manage.section": "Manage section",
            PAYMENT_METHOD:{
                TITLE: 'Payment method',
                UPDATE:{
                    SUCCESS:'Payment method updated.',
                },
                BANK_DETAILS:{
                    NOT_ADDED: 'Bank Details are not provided please update Bank Transfer Details.'
                }
            },
            PAYMENT_CURRENCY:{
                TITLE: 'Payment currency'
            },
            NFT: {
                TITLE: "NFT Colleciton"
            }
        },
        EDIT_PROJECT: {
           TITLE: "Project Members",
           COLUMNS: {
                STATUS: "Status",
                ROLE: "Role",
                ACTIONS: 'Actions'
           },
           ACTIONS: {
                ADD: "Add new member",
                REMOVE: "Remove"
           },
           ROLE: {
                OWNER: "Owner",
                MEMBER: "Member"
           },
           STATUS: {
                ACCEPTED: "Accepted",
                WAITING: "Waiting"
           },
           REMOVE_FORM: {
                TITLE: "Confirm the action",
                DESCRIPTION: "Are you sure you want to delete users from the project?",
                ACTIONS: {
                    SUBMIT: "Submit",
                    CANCEL: "Cancel"
                }
           },
           ALERTS: {
                REGULAR_VALIDATOR: "Invalid user email",
                SUCCESSFULLLY_ADDED: "User was successfully added",
                SUCCESSFULLY_REMOVED: "Member was successfully removed",
                SUCCESSFULLY_UPDATED: "Successfully updated member"
           }
        },
        FORM: {
            "lbl.name": "Name",
            "lbl.role": "Role",
            "lbl.facebook": "Facebook",
            "lbl.twitter": "Twitter",
            "lbl.linkedin": "Linkedin",
            "url.invalid": "Invalid URL (pattern: https://example.com)",
            "allowed_file": "Allowed file formats: JPEG, JPG, PNG, GIF",
            "move.down": "Move down once",
            "move.up": "Move up once",
            "edit": "Edit",
            "delete": "Delete",
            CHANGE_AVATAR : 'Change avatar',
            CANCEL_AVATAR : 'Cancel avatar',
            REMOVE_AVATAR : 'Remove avatar'
        },
        PROJECT_TEAM: {
            TITLE: "Meet our team",
            SUBTITLE: "Add People who contribute to the project, let your investor know you.",
            EMAIL_ENTER: "Enter user's email",
            BTN_SAVE: "Save Member",
            BTN_CREATE: "Create",
            BTN_CLOSE: "Close",
            BTN_CANCEL: "Cancel",
            SAVED_SUCCESSFULLY: "Partner data has been saved successfully.",
            MANAGE_PEOPLE: "Manage People who contribute to the project, let your investor know you.",
            ADD: "Add new member",
            EDIT: 'Edit team member',
            DELETE: 'Delete team member',
            DELETED: 'Member deleted',
            MOVE_LEFT: 'Move left',
            MOVE_RIGHT: 'Move right',
            INVALID_NAME: 'Invalid member name',
            INVALID_ROLE: 'Invalid member role'
        },
        PROJECT_PARTNER: {
            TITLE: "Partners",
            SUBTITLE: "Manage people who contribute to the project, let your investor know you.",
            BTN_SAVE: "Save Partner",
            BTN_CLOSE: "Close",
            BTN_CANCEL: "Cancel",
            SAVED_SUCCESSFULLY: "Partner data has been saved successfully.",
            ADD: "Add new partner",
            EDIT: 'Edit partner',
            DELETE: 'Delete partner',
            MOVE_LEFT: 'Move left',
            MOVE_RIGHT: 'Move right',
            INVALID_NAME: 'Invalid partner name',
            INVALID_ROLE: 'Invalid partner role'
        },
        PROJECT_OVERVIEW: {
            FORM: {
                DESCRIPTION: {
                    LABEL: "Project description",
                    PLACEHOLDER: "Describe shortly what your project is about",
                    ERROR: "Enter valid description"
                },
                TOKEN: {
                    LABEL: "Select token"
                },
                ABOUT: {
                    LABEL: "About project",
                    PLACEHOLDER: "Describe shortly what your project is about",
                    ERROR: "Enter valid description",
                    ACTIONS: {
                        SAVE: "Save",
                        CANCEL: "Cancel"
                    }
                }
            },
            NO_TOKENOMICS_AVAILABLE: "Tokenomics are unavailable",
            EXCHANGES: {
                NO_EXCHANGES: "No exchange was configured for this project",
                NO_TOKEN: 'Cannot load exchanges. You need to choose the token first',
                TITLE: 'Exchange',
                MESSAGES: {
                    ACTIVATED: "Exchange was successfully activated",
                    DEACTIVATED: "Exchange was successfully deactivated"
                }
            },
            VIEW_DETAILS: "View details",
            FEATURES: {
                TITLE: "Features",
                POS: "POS",
                POS_HINT: "The mechanism to receive a reward in exchange for frozen tokens. The reward is generated on a periodical basis and split between all stakers proportionally to the amount of owned frozen tokens. The reward can be represented by USDT or other token and will be deposit directly to user wallet.",
                VESTING: "Vesting",
                VESTING_HINT: "Vesting is the process of locking and releasing tokens after a given time. Just like in traditional finance, vesting in the crypto world is often used to ensure long-term commitment to a project from team members.",
                MINT: "Mint",
                MINT_HINT: "Generation of new tokens",
                BURN: "Burn",
                BURN_HINT: "The act of burning tokens effectively removes tokens from the available supply, which decreases the number in circulation.",
                INVESTMENT_PACKAGES: "Investment packages",
                INVESTMENT_PACKAGES_HINT: "Amount of tokens offered in preferable price or with additional asset as service or product of and Investee.",
                DEFLATION: "Deflation",
                DEFLATION_HINT: "Deflation is the processes of burnining tokens during some operation. For example, during a transaction some % of tokens can be automatically burned",
                STATES: {
                    ENABLED: "Enabled",
                    DISABLED: "Disabled",
                    AVAILABLE: "Available",
                    UNAVAILABLE: "Unavailable"
                }
            },
            SALES_INFO: {
                TITLE: "Sales info",
                TOKEN_PRICE: "Token price",
                MIN_INVEST: "Min. invest",
                MAX_INVEST: "Max. invest",
                SOFT_CAP: "Soft cap",
                HARD_CAP: "Hard cap",
                TOTAL_SUPPLY: "Total supply"
            },
            TOKENOMICS: {
                TITLE: "Tokenomics"
            },
            SIDEBAR: {
                ADDRESS: "Address",
                ADDRESS_COPIED_MESSAGE: 'Address copied!',
                INVESTOR_CERTIFICATE: 'Investor certificate',
                INVESTOR_CERTIFICATE_LOADING: 'Your certificate is on the way'
            },
            STAGES: {
                TITLE: "Project stages",
                TABLE: {
                    STAGE: "Stage",
                    START_DATE: "Start Date",
                    END_DATE: "End Date",
                    TOKEN_PRICE: "Token price",
                    TOKEN_SUPPLY: "Token supply",
                    NO_STAGES: "Stages are not configured"
                }
            },
            ACTIONS: {
                NEW_TOKEN: "Create new token",
                EDIT: "Edit",
                DELETE: "Delete",
                EDIT_LOGO: 'Edit logo',
                ADD_COVER_PHOTO: 'Add cover (picture)',
                EDIT_COVER_PHOTO: 'Edit cover (picture)',
                ADD_COVER_PHOTO_SHORT: 'Add cover',
                EDIT_COVER_PHOTO_SHORT: 'Edit cover',
                TITLE_EDIT_COLORS: 'Set the main colors for the project',
                SET_COLORS: 'Set the main colors',
                SET_COLORS_SHORT: 'Set the colors',
                TITLE_1_EDIT_COLORS: 'Primary color of the project',
                TITLE_1_DESC_EDIT_COLORS: 'Click below on the color bar to display the color palette',
                TITLE_2_EDIT_COLORS: 'Second (complementary) color of the project',
                TITLE_2_DESC_EDIT_COLORS: 'Click below on the color bar to display the color palette',
                TITLE_3_EDIT_COLORS: 'Text color on the cover background',
                TITLE_3_DESC_EDIT_COLORS: 'Click below on the color bar to display the color palette',
                VIEW_DETAILS: "View details",
                EDIT_SOCIAL_MEDIA: "Edit social media",
                ADD_SOCIAL_MEDIA: "Add social media"
            },
            MESSAGES: {
                INVALID_FORM: 'Form has invalid values. Please, fix them to continue',
                SHORT_DESC_UPDATED: 'Description was successfully updated',
                TITLE_UPDATED: 'Project title was successfully updated',
                TOKEN_UPDATED: 'Token was successfully updated',
                COLORS_UPDATED: 'The main colors have been updated',
                CONTENT_UPDATED: 'Content was updated successfully',
                SOCIAL_MEDIA_UPDATED: 'Project Social Media has been updated',
                EMPTY_DATA: 'The project description is missing, please fill it in to continue.'
            },
            FILES: {
                ADD_DOCUMENT: "Upload document",
                EDIT_DOCUMENT: "Edit document",
                CREATE_DOCUMENT: 'Create document',
                SELECT_DOCUMENT_LANGUAGE: "Select document language",
                LANGUAGE: {
                    ENGLISH: "English",
                    POLISH: "Polish"
                },
                DOWNLOAD: 'Download',
                MESSAGES:{
                    DOCUMENT_SAVE: 'Document was successfully created',
                }
            },
            SETTINGS: {
                TITLE: 'Project settings'
            },
            LINKS: {
                POS: 'https://docs.mosaico.ai/fundamentals/the-investment/what-is-a-pos',
                VESTING: 'https://docs.mosaico.ai/fundamentals/the-investment/what-is-vesting',
                MINT: 'https://docs.mosaico.ai/fundamentals/mint-vs.-burn./what-is-mint',
                BURN: 'https://docs.mosaico.ai/fundamentals/marketplace/what-is-a-total-supply',
                INVESTMENT_PACKAGES: 'https://docs.mosaico.ai/fundamentals/the-investment/what-are-the-investment-packages',
                DEFLATION: 'https://docs.mosaico.ai/fundamentals/marketplace/what-is-deflation',
                TOKEN_PRICE: 'https://docs.mosaico.ai/fundamentals/marketplace/what-is-the-price-of-a-token',
                MIN_INVEST: 'https://docs.mosaico.ai/fundamentals/the-investment/what-is-a-min.-investment',
                MAX_INVEST: 'https://docs.mosaico.ai/fundamentals/the-investment/what-is-a-max-investment',
                SOFT_CAP: 'https://docs.mosaico.ai/fundamentals/soft-cap-vs.-hard-cap./what-is-a-soft-cap',
                HARD_CAP: 'https://docs.mosaico.ai/fundamentals/soft-cap-vs.-hard-cap./what-is-a-hard-cap',
                TOTAL_SUPPLY: 'https://docs.mosaico.ai/fundamentals/marketplace/what-is-a-total-supply',
                STAGES: 'https://docs.mosaico.ai/fundamentals/marketplace/what-are-the-stages-of-a-project',
                TOKENOMICS: 'https://docs.mosaico.ai/fundamentals/marketplace/what-is-a-tokenomic'
            }
        },
        PROJECT_MEMBERS: {
            CANNOT_REMOVE_YOURSELF: "You cannot remove yourself",
            CANNOT_REMOVE_PROJECT_CREATOR: "You cannot remove the project creator"
        },
        SOCIAL_LINKS: {
            CARD: {
                TITLE: "Your project's social media",
                TELEGRAM: 'Telegram Channel',
                YOUTUBE: 'Youtube Channel',
                LINKEDIN: 'LinkedIn Profile',
                FACEBOOK: 'Facebook Page',
                TWITTER: 'Twitter Profile',
                INSTAGRAM: 'Instagram Profile',
                MEDIUM: 'Medium',
                TIKTOK: 'TikTok'
            },
            MESSAGE: {
                SUCCESS: 'Social media liks has been updated.',
                FAILED: 'Data not saved! Please check and try again.',
                INVALID_URL: 'You entered invalid URL (URL must start with https)'
            }
        },
        PROJECT_FOOTER: {
            COMPANY: {
                MORE_DETAILS: 'Check more'
            }
        },
        BUY: {
            back_to_project: 'Back to the project'
        },
        EDITOR: {
            ACTIONS: {
                SAVE: "Save",
                RESET: "Reset",
                CANCEL: "Cancel"
            },
            RESET: {
                TITLE: "Need confirmation",
                MESSAGE: "Are you sure you want to quit? All your progress will be lost.",
                ACTIONS: {
                    CONFIRM: "Yes",
                    CANCEL: "No"
                }
            },
            MESSAGES: {
                SUCCESS: 'Document was saved successfully',
                INVALID_VALUE: 'Cannot save empty content'
            }
        },
        PROJECT_SETTINGS: {
            TRANSACTIONS: {
                TITLE: 'Transactions',
                COPY_HINT: 'Copy to clipboard',
                TABLE: {
                    COLUMNS: {
                        USER_NAME: 'User Name',
                        PROJECT_NAME: 'Project Name',
                        TRAN_ID: 'Transaction ID',
                        PACKAGE_NAME: 'Package Name',
                        PAID: 'Paid',
                        PURCHASE_DATE: 'Date of purchase',
                        BALANCE: 'Balance $',
                        BALANCE_GOODS: 'Balance goods',
                        SOURCE: 'Paym. Meth.'
                    },
                },
                NO_TRANSACTIONS: 'There are no transactions',
                MESSAGES: {
                    NO_TRANSACTIONS_TO_BE_EXPORTED: 'There are no transactions to be exported'      
                }
            },
            INVESTORS: {
                TITLE: 'Investors',
                COPY_HINT: 'Copy to clipboard',
                TABLE: {
                    COLUMNS: {
                        USER_NAME: 'User name',
                        PHONE_NUMBER: 'Phone number',
                        WALLET_ADDRESS: 'Wallet address',
                        USDT_BALANCE: 'USDT balance',
                        USDC_BALANCE: 'USDC balance',
                        MATIC_BALANCE: 'MATIC balance',
                        BANK_TRANSFER: 'Bank Transfer',
                        TOTAL_INVESTMENT: 'Total Investment'
                    }
                },
                NO_INVESTORS: 'There are no investors to display.'
            }
        },
        MODALS: {
            NEW_TOKEN: {
                TITLE: 'Create token'
            },
            CAMPAIGN_EDITOR: {
                TITLE: 'Campaign',
                FORM: {
                    HARDCAP: {
                        LABEL: 'Hard cap',
                        ERROR: 'Invalid value',
                        PLACEHOLDER: 'Enter your goal'
                    },
                    SOFTCAP: {
                        LABEL: 'Soft cap',
                        ERROR: 'Invalid soft cap',
                        PLACEHOLDER: ''
                    },
                    NAME: {
                        LABEL: "Stage name",
                        ERROR: "Invalid name",
                        PLACEHOLDER: "Enter stage name"
                    },
                    SUPPLY: {
                        LABEL: "Token supply",
                        PLACEHOLDER: 'Enter amount of tokens dedicated for this round',
                        ERROR: 'Invalid value'
                    },
                    TOKEN_PRICE: {
                        LABEL: "Token price",
                        PLACEHOLDER: 'Enter token price for this round',
                        ERROR: 'Invalid value'
                    },
                    MIN_PURCHASE: {
                        LABEL: "Minimum purchase",
                        PLACEHOLDER: 'Enter minimum amount of tokens to purchase',
                        ERROR: 'Invalid value'
                    },
                    MAX_PURCHASE: {
                        LABEL: "Maximum purchase",
                        PLACEHOLDER: 'Enter maximum amount of tokens to purchase',
                        ERROR: 'Invalid value'
                    },
                    START_DATE: {
                        LABEL: "Start date",
                        PLACEHOLDER: 'Choose the date',
                        ERROR: 'Invalid value'
                    },
                    END_DATE: {
                        LABEL: "End date",
                        PLACEHOLDER: 'Choose the date',
                        ERROR: 'Invalid value'
                    },
                    PRIVATE_SALE: {
                        LABEL: 'Private sale'
                    },
                    ACTIONS: {
                        SAVE: 'Save',
                        ADD: 'Add stage',
                        DELETE: 'Delete'
                    }
                },
                MESSAGES: {
                    UPDATE_SUCCESS: "Campaign was successfully updated",
                    INVALID_FORM: "Form contains invalid values"
                }
            },
            PROJECT_LOGO_EDITOR: {
                TITLE: "Edit project logo",
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
            COVER_UPLOAD: {
                TITLE: "Project cover image",
                ACTIONS: {
                    ADD: "Add",
                    CHANGE: "Change"
                },
                MESSAGES: {
                    SUCCESS: "Cover was successfully updated",
                    INVALID_FORM: "There is invalid data. Cannot save"
                }
            },

            PROJECT_ARTICLE_PHOTO_UPLOAD: {
                TITLE: "Edit author photo",
                ACTIONS: {
                    SAVE: "Save",
                    CANCEL: 'Cancel',
                    ADD: 'Add author photo',
                    CHANGE: 'Edit author photo'
                },
                MESSAGES: {
                    SUCCESS: "Author photo was successfully updated",
                    INVALID_FORM: "There is invalid data. Cannot save"
                }
            },
            PROJECT_ARTICLE_COVER_UPLOAD: {
                TITLE: "Edit cover picture",
                ACTIONS: {
                    SAVE: "Save",
                    CANCEL: 'Cancel',
                    ADD: 'Add cover picture',
                    CHANGE: 'Edit cover picture'
                },
                MESSAGES: {
                    SUCCESS: "Cover picture was successfully updated",
                    INVALID_FORM: "There is invalid data. Cannot save"
                }
            },
            SUBSCRIPTION_TO_NEWSLETTER: {
                TITLE: "Sign up for an investor alert",
                ACTIONS: {
                    SAVE: "Sign up",
                    CANCEL: 'Cancel',
                    SIGN_ME_OUT: 'Sign me out',
                    // ADD: 'Add logo',
                    // CHANGE: 'Edit logo'
                },
                MESSAGES: {
                    INFO: "Do you want to learn more about the project? To be one of the first investors? Sign up for our whitelist!",
                    INFO_FOR_NOT_LOGGED_IN: "For logged in only for now!",
                    INFO_FOR_NOT_LOGGED_IN2: "Log in",
                    INFO_ALREADY_SUBSCRIBED: "You are already subscribed to the project newsletter.",
                    INFO_SUBSCRIBED_TO_NEWSLETTER_LOGGED_IN: "Subscribed to the newsletter!",
                    INFO_SUBSCRIBED_TO_NEWSLETTER_NOT_LOGGED_IN: "Receive a message from us and click on the link to confirm that you want to subscribe to the newsletter.",
                    INFO_UNSUBSCRIBED_TO_NEWSLETTER: "Unsubscribed from the newsletter",
                    INFO_CONFIRM_UNSUBSCRIBED: "Do you want to unsubscribe from the newsletter?",
                }
            },
            EDIT_BANK_DATA: {
                TITLE: "Edit bank transfer details",
                SUBTITLES: {
                    ACCOUNT_NUMBER: "Account number",
                    BANK_NAME: "Bank name",
                    SWIFT: "Swift",
                    ACCOUNT_ADDRESS: "Account address"
                },
                PLACEHOLDERS: {
                    ENTER_ACCOUNT: "Enter account",
                    ENTER_BANK_NAME: "Enter bank name",
                    ENTER_SWIFT: "Enter swift",
                    ENTER_ACCOUNT_ADDRESS: "Enter account address"
                },
                MESSAGES: {
                    SUCCESS: "You successfully updated bank transfer details"
                },
                ACTIONS: {
                    UPDATE: "Update",
                    CANCEL: "Cancel"
                },
                FORM:{
                    ACCOUNT: {
                        ERROR: "Required field",
                    },
                    BANK_NAME: {
                        ERROR: "Required field"
                    },
                    SWIFT: {
                        ERROR: "Required field"
                    },
                    ACCOUNT_ADDRESS: {
                        ERROR: "Required field"
                    },
                    MESSAGES: {
                        ERROR: "Form has invalid values. Please fix issue to continue"
                    },
                }
            },
            STAKING_TOKEN_CLAIM: {
                MESSAGES: {
                    CLAIM_TOKEN_COST: "You claim tokens before finishing of the  contract, it will cost you a fee and you will loose the reward.",
                    ARE_YOU_SURE: "Are you sure you would like to withdraw the funds?",
                    FEE_COST: "Fee cost 30.00 MOS",
                    DO_YOU_WANT: "Do you want to exchange all WMOS to MOS?",
                    YOU_WILL_RECIEVE: "You will recieve ... MOS ",
                    SUCCESSFULY_EXCHANGED: "You successfuly exchanged WMOS to MOS.",
                    PLEASE_NOTE: "Please note it may take up to 48h to appear on your wallet."
                },
                ACTIONS: {
                    CLAIM: "Claim",
                    CANCEL: "Cancel",
                }
            }
        },
        PROJECT_SCORE: {
            TITLE: "Complete project information to start your campaign!",
            DESCRIPTION: "Filling in all necessary information will help your project to be more attractive to diverse groups of people.",
            ACTIONS: {
                EXPAND_ERRORS: "What is missing?",
                SUBMIT: "Submit"
            }
        },
        SUBSCRIPTION_TO_NEWSLETTER: {
            TITLE: "Zapisz się na alert inwestora",
            ACTIONS: {
                SAVE: "Subscribe",
                CANCEL: 'Cancel',
                SIGN_ME_OUT: 'Unsubscribe',
                // ADD: 'Add logo',
                // CHANGE: 'Edit logo'
            },
            MESSAGES: {
                INFO: "Want to learn more about the project? Be one of the first investors? Sign up for our whitelist!",
                INFO_FOR_NOT_LOGGED_IN: "Narazie tylko dla zalogowanych!",
                INFO_FOR_NOT_LOGGED_IN2: "Zaloguj się",
                INFO_ALREADY_SUBSCRIBED: "Jesteś juz zapisany do newslettera projektu.",
                INFO_SUBSCRIBED_TO_NEWSLETTER_LOGGED_IN: "You subscribed to the newsletter!",
                INFO_SUBSCRIBED_TO_NEWSLETTER_NOT_LOGGED_IN: "Go to your mailbox and click on the link in it to confirm your subscription to the newsletter.",
                INFO_UNSUBSCRIBED_TO_NEWSLETTER: "Wypisano z newslettera!",
                INFO_CONFIRM_UNSUBSCRIBED: "Would you like to unsubscribe from the newsletter?",
            }
        },
        PRIVATE_SALE_SUB: {
            ENABLED: "Congratulations! You are now able to purchase tokens of",
            ACTIONS: {
                PURCHASE: "Buy tokens"
            }
        },
        VALIDATION_NO_DESCRIPTION: "Add project description in 'About' section",
        VALIDATION_NOT_FILLED_COMPANY: "Add more information about your company. Complete your company verification.",
        VALIDATION_NO_PAGE_COVER: "Change cover image",
        VALIDATION_NO_STAGES: "Add sales campaign in 'Settings' section",
        VALIDATION_NO_TOKEN: "Choose a token you want to sell",
        VALIDATION_NO_TITLE: "Add project title",
        VALIDATION_NO_SHORT_DESCRIPTION: "There is no short description. Help investors to find your project faster",
        VALIDATION_NO_FAQ: "You don't have any FAQ item in the project. Add several of them to answer most common questions about your project",
        VALIDATION_INVALID_STAGE_START_DATE: "Update your sale campaing",
        VALIDATION_NO_CROWDSALE: "Crowdsale contract not deployed",
        INVALID_PROJECT: "There is missing information. Check the project score and fix suggested issues in order to proceed",
        PROJECT_JOINED: "You successfully joined the project",
        VALIDATION_NO_DOCUMENTS: "Make sure you provided at least 3 documents: Whitepaper, Terms & Conditions, and Privacy Policy",
        VALIDATION_NO_TEAM: "The community usually demands team members to be transparent and known. Add several team members in the 'About' section",
        VALIDATION_NO_SOCIAL_MEDIA: 'Make sure you take proper care about your social media. Add references to Facebook, Telegram, etc.',
        INVALID_SOFTCAP: "Invalid soft cap",
        INVALID_HARDCAP: 'Invalid hard cap',
        VALIDATION_NO_HARDCAP_SOFTCAP: 'Specify the goal of your project. Remember that soft cap should be at least 30% of the hard cap.',
        PACKAGES_Add_new_package: "Add new package",
        PACKAGES_Edit_list: "Package list",
        PACKAGES_Edit_package: "Edit the package",
        PACKAGE_ADDED: "Package details were successfully added",
        PACKAGE_DELETED: "Investment package has been deleted",
        MODAL_PACKAGE_title: "Investment package",
        INFO_UNDER_IMAGE: 'Change default picture (optional)',
        PACKAGE_addPhotoText: 'Add picture',
        PACKAGE_changePhotoText: 'Change picture',
        PACKAGE_package_name: 'Package name',
        PACKAGE_placeholder_package_name: 'Enter package name',
        PACKAGE_incorect_package_name: 'Invalid package name',
        PACKAGE_invalid_form: 'Form has invalid values. Please fix issue to continue.',
        PACKAGE_tokens_amount: 'Tokens amount',
        PACKAGE_placeholder_tokens_amount: 'Enter token amount',
        PACKAGE_incorect_tokens_amount: 'Invalid tokens amount',
        PACKAGE_benefits_title: 'Benefits',
        PACKAGE_benefits_title_info: 'You can add up to 5 benefits',
        PACKAGE_benefit_1: 'Benefit 1',
        PACKAGE_benefit_2: 'Benefit 2 (optional)',
        PACKAGE_benefit_3: 'Benefit 3 (optional)',
        PACKAGE_benefit_4: 'Benefit 4 (optional)',
        PACKAGE_benefit_5: 'Benefit 5 (optional)',
        PACKAGE_placeholder_benefit_1: '',
        PACKAGE_placeholder_benefit_2: '',
        PACKAGE_placeholder_benefit_3: '',
        PACKAGE_placeholder_benefit_4: '',
        PACKAGE_placeholder_benefit_5: '',
        PACKAGE_incorect_benefit: 'Invalid benefit name',
        PACKAGE_no_list: 'There are no added investment packages',
        PRESS_ROOM_invalid_form: 'Form has invalid values. Please fix issue to continue.',
        TOKEN_NOT_DEPLOYED: "Token not deployed. Make sure token was created on blockchain",
        INSUFFICIENT_FUNDS: "You have insufficient funds",
        GAS_TOO_EXPENSIVE: "Gas is too expensive",
        MODAL_FAQ_title: "FAQ",
        FAQ_Add_new_faq: "Add new FAQ",
        FAQ_Edit_list: "Faq list",
        FAQ_Edit_faq: "Edit",
        FAQ_DELETED: "FAQ was successfully deleted",
        FAQ_question_name: 'Displayed title',
        FAQ_placeholder_question_name: 'Add popular question here',
        FAQ_incorect_question_name: 'Invalid value',
        FAQ_answer_name: 'Dropdown answer',
        FAQ_placeholder_answer_name: 'Paste answer here',
        FAQ_incorect_answer_name: 'Invalid value',
        FAQ_is_Faq_hidden: 'Is faq hidden?',
        FAQ_no_list: 'There is no FAQ',
        FAQ_delete_title: 'Are you sure you want to delete an FAQ?',
        FAQ_delete_description: 'It will be removed for forever ',
        PROJECT_PURCHASE: {
            RAMP: "Pay with Ramp",
            TRANSAK: "Pay with Transak",
            INIT_BANK_TRANSFER: "Get payment details",
            CONFIRM_BANK_TRANSFER: "Confirm",
            METAMASK: "Pay",
            COPY_SUCCESS: 'Copied',
            COPY_HINT: "Copy to clipboard",
            TRANSACTION_INIT: "Transaction was initiated. Please, wait until it is confirmed.",
            BANK_TRANSFER: {
                REMARK: "* Your deposit will be processed within 8 business hours from the time the funds are posted to our bank account.",
                SWIFT: "SWIFT/BIC",
                DETAILS: "Transfer details",
                ACCOUNT: "To account",
                REFERENCE: "Reference / Title",
                RECIPIENT: "Recipient",
                BANK: "Bank name",
                KYC_ALERT: {
                    TITLE: "KYC is required",
                    CONTENT: "KYC is a required procedure of identity verification in order to prevent fraud, corruption, money laundering and terrorist financing. You have to complete the verification before continuation.",
                    COMPLETE: "Verify identity"
                }
            }
        },
        CHECKOUT: {
            TOKENS: "tokens",
            MIN_TOKENS: "You have to buy min.",
            MAX_TOKENS: "You can buy max.",
            REQUIRED_FIELD: 'Required field'
        },
        CREDIT_CARD_PAYMENT: {
            EXPLAINER: {
                TITLE: "Information on fees",
                ACTIONS: {
                    ACCEPT: "Accept",
                    ACCEPT_NO_SHOW: "Accept and do not show anymore"
                },
                CONTENT: "<p>This <strong>payment processor will exchange your chosen currency into stablecoin</strong> to be able to purchase project tokens. </p> <p>Please, keep in mind, that due to <strong>dynamically changing currency values, the number of tokens might slightly differ.</strong> </p> <p>If you would like to be on the safe side, order a couple of tokens more.</p> <p>You can ignore displayed currency values and presented fees in the payment processor.</p><p>Calculated fees are just information given automatically by an external provider.</p> <p>You do not pay any additional fees. All of the displayed fees are covered by the project creator.</p>"
            }
        },
        PRESS_ROOM: {
            TITLE: "Press room",
            ACTIONS: {
                ADD_NEW: "Add new",
                READ_MORE: "Read more"
            },
            TITLE_ADD: 'Add new article',
            TITLE_EDIT: 'Edit article',
            DELETE_TITLE: 'Are you sure you want to delete an article?',
            DELETE_DESCRIPTION: 'It will be removed for forever',
            ADD_COVER: 'Add cover picture',
            ARTICLE_LINK: 'Displayed link',
            INCORRECT_ARTICLE_LINK: 'Incorrect article link',
            TEXT_VISIBLE: 'Displayed title',
            IMG_VISIBLE: 'Displayed image',
            INCORRECT_TEXT_VISIBLE: 'Incorrect text',
            ADDITIONAL_INFO: 'Additional info',
            ADD_AUTHOR_PHOTO: 'Add author photo (optional)',
            CHANGE_AUTHOR_PHOTO: 'Change Author Photo',

            ADD_ARTICLE_AUTHOR: 'Add article author name (optional)',
            INCORRECT_ARTICLE_AUTHOR: 'Incorrect article author name',

            ADD_ARTICLE_DATE: 'Add article date (optional)',
            INCORRECT_ARTICLE_DATE: 'Incorrect date',
            ARTICLE_NO_LIST: 'There are no articles yet',
            ARTICLE_HIDDEN: 'Hidden',
        },
        ARTICLE:{
            MESSAGES:{
                HIDE_SUCCESS:'Article was hidden successfully',
                DISPLAY_SUCCESS: 'Article was displayed successfully',
                CREATE_SUCCESS: 'Article was successfully created',
                UPDATE_SUCCESS: 'Article was successfully updated'
            }
        },
        "ORDER_CONFIRMATION": {
            "CONTENT": "Thank you for your purchase. Please wait for information about a status of the transaction.",
            "ACTIONS": {
                "INVEST": "Invest again",
                "CHECK_TX": "Check transaction"
            }
        },
        "YOUTUBE": "YouTube",
        "LINKEDIN": "LinkedIn",
        "FACEBOOK": "Facebook"
    }
};

export const locale = {
    lang: 'en',
    data: {
        MANUAL_DEPOSIT: {
            TITLE: "Manual transfer",
            ACTIONS: {
                DONE: "Done",
                COPY_HINT: "Copy address"
            },
            ADDRESS_COPIED: "Address copied",
            INFO: "Please, perform the manual transfer of desired tokens (Matic, <a href='https://polygonscan.com/token/0xc2132d05d31c914a87c6611c10748aeb04b58e8f' target='_blank'>USDT</a> or <a href='https://polygonscan.com/token/0x2791bca1f2de4661ed88a30c99a7a9449aa84174' target='_blank'>USDC</a>) to the address below. <span class='fw-bold'>The transfer should be performed through the Polygon network.</span> Your balance will be automatically updated within 5 minutes after transaction confirmation."
        },
        WALLET_OVERVIEW: {
            DASHBOARD: "Dashboard",
            STAKING: "Staking",
            VESTING: "Vesting",
            LEARN_MORE: "Learn more",
            AFFILIATION: "Affiliation"
        },
        WALLET_STAKING: {
            FORM: {
                ACCEPT_WITH_TERMS: "I have read and I accept <a href='{{termsAndConditionsUrl}}' target='_blank'>Terms and Conditions</a>",
                ACCEPT_WITHOUT_TERMS: 'I understand and accept possible risk related to the staking'
            },
            DISCLAIMER: {
                READ_MORE: "read more",
                STANDARD: {
                    SHORT: "This staking freezes your tokens for an undefined period of time...",
                    FULL_INFO: "This staking freezes your tokens for an undefined period of time. You can withdraw tokens any time, but you will not be eligable for next reward release. Adding tokens to this staking is possible and it will influence the amount of reward you will be eligible for, in accordance with the principles described in the whitepaper. Estimated APR is calculated based on Project’s whitepaper and declared periodical yield of the issuer. The declared yield will be verified shortly before the next reward drop, based on actual values provided by the issuer.",
                    TITLE: "Staking information"
                }
            },
            STATISTICS: {
                TITLE: {
                    TOTAL_IN_STAKING: "Total in Staking",
                    ACTIVE_STAKING: "Active Staking",
                    REWARD_CLAIMED: "Reward Claimed"
                }
            },
            ASSETS: {
                AMOUNT: {
                    PLACEHOLDER: 'Amount'
                },
                TITLE: {
                    STAKED_ASSETS: "Select Asset",
                    ASSET_AMOUNT: "Amount",
                    TYPE: "Staking type",
                    SELECT_MAX: "MAX",
                    PERIOD_ASSETS: "Period to be Staked",
                    ESTIMATED_APR: "Estimated APR",
                    ESTIMATED_TOKEN: "Estimated token reward",
                    ESTIMATED_USD: "Estimated USD reward"
                },
                WARNING: "Activating Staking blocks the funds for the time specified in the contract. Claiming funds earlier will incur additional fees.",
                SUBTITLE: {
                    AVALABLE_BALANCE: "Balance",
                    STAKING_PERIOD: "Staking period"
                }
            },
            TOP_STAKING: {
                STAKING_TITLE: "Top tokens",
                NO_TOP_STAKINGS: "Here you will see tokens with highest APR",
                ACTIONS: {
                    STAKE_NOW: "Stake now",
                    BUY_TOKENS: "Buy tokens"
                }
            },
            LEARN_MORE: "Learn more",
            VIEW_ALL: "View all",
            PANEL: {
                TITLE: {
                    ACTIVE: "Active",
                    STAKING_HISTORY: "Staking history"
                },
                ACTIVE: {
                    PAID_WALLET: "Paid from wallet",
                    TITLE: {
                        NEXT_REWARD: "Next reward",
                        STAKED_VALUE: "Staked value",
                        ESTIMATED_REWARD: 'Estimated reward'
                    },
                    NO_TOKENS: "Here you will see your active stake",
                    ACTIONS: {
                        CLAIM_TOKENS: "Claim reward",
                        WITHDRAW: 'Finish staking'
                    }
                },
                HISTORY: {
                    TABLE: {
                        TOKEN_NAME: "Token name",
                        STATUS: "Status",
                        STAKED: "Staked",
                        REWARDED: "Rewarded",
                        APR: "APR",
                        START_DATE: "Start date",
                        END_DATE: "End date"
                    },
                    NO_STAKINGS: "Brak danych"
                }
            }
        },
        WALLET_VESTING: {
            TITLES: {
                CLAIMED: "Claimed",
                TOTAL_PERIOD: "Total period",
                LOCKED: "Locked",
                NEXT_UNLOCK: "Next unlock"
            },
            NO_VESTING: "No tokens in vesting",
            ACTIONS: {
                CLAIM: "Claim",
                TOKENS: "tokens"
            },
            SUCCESS: {
                TITLE: "You successfully claimed",
                TOKENS: "tokens.",
                DESCRIPTION: "It will appear on your wallet in a few minutes."
            }
        },
        WALLET_PAGE_MENU: {
            WALLET: 'Wallet'
        },
        USER_WALLET: {
            SUMMARY: {
                TOTAL: 'Total value',
                COPY_HINT: 'Copy to clipboard',
                NETWORK: {
                    LABEL: 'Blockchain network',
                    PLACEHOLDER: "Choose your blockchain"
                },
                ACTIONS: {
                    DEPOSIT: 'Deposit',
                    SEND: 'Send',
                    WITHDRAW: 'Withdraw',
                    MANUAL_DEPOSIT: 'Manual transfer'
                },
                MESSAGES: {
                    COPIED: "Copied"
                }
            },
            OVERVIEW: {
                STAKING: {
                    TITLE: 'Staked value',
                    COPY_HINT: 'Value of all staked tokens presented in currency',
                    REWARD_DATE_TITLE: 'Next reward',
                    ACTIONS: {
                        VIEW_DETAILS: 'View details',
                        DEPOSIT: 'Deposit',
                        WITHDRAW: 'Withdraw'
                    },
                    MESSAGES: {
                    }
                },
                VESTING: {
                    TITLE: 'Locked in vesting',
                    COPY_HINT: 'Vesting is the process of locking and releasing tokens after a given time. Just like in traditional finance, vesting in the crypto world is often used to ensure long-term commitment to a project from team members',
                    REWARD_DATE_TITLE: 'Next reward',
                    ACTIONS: {
                        VIEW_DETAILS: 'View details'
                    },
                    MESSAGES: {
                    }
                },
                PACKAGES: {
                    TITLE: 'Locked in packages',
                    COPY_HINT: 'Value of all services or products coming from packages added to previously purchased tokens presented in currency',
                    ACTIVE_PACKAGES_TITLE: 'Active packages',
                    ACTIONS: {
                        VIEW_DETAILS: 'View details'
                    },
                    MESSAGES: {
                    }
                }
            },
            PANEL: {
                ASSETS: {
                    TITLE: 'Assets',
                    NO_ASSETS: 'There are no assets. Time to change it...',
                    ACTIONS: {
                        EXCHANGE: 'Exchange',
                        STAKE: 'Stake',
                        MANAGE: 'Manage'
                    }
                },
                TRANSACTIONS: {
                    TITLE: 'Transactions',
                    TABLE: {
                        TOKEN: 'Token name',
                        HASH: 'Transaction',
                        SOURCE: 'Source',
                        AMOUNT: 'Amount',
                        DESTINATION: 'Destination'
                    },
                    NO_TRANSACTIONS: 'There are no transactions',
                    ACTIONS: {
                        LOAD_MORE: 'View more'
                    }
                },
                KANGA_WALLET: {
                  TITLE: 'Kanga wallet'
                }
            }
        },
        MODALS: {
            WALLET_SEND: {
                TITLE: 'Send',
                ACTIONS: {
                    SEND: 'Send',
                    CANCEL: 'Cancel',
                    CLOSE: "Done"
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
                    TRANSACTION_INITIATED: "Transaction was initiated. Please, wait until it is processed.",
                    SUCCESS: 'Assets were successfully transferred'
                }
            },
            TOKEN_LOGO_EDITOR: {
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
            }
        },
        INSUFFICIENT_FUNDS: "Insufficient balance",
        TOKEN_MANAGEMENT: {
            FEATURES: {
                STATES: {
                    ENABLED: "Enabled",
                    DISABLED: "Disabled",
                    AVAILABLE: "Available",
                    UNAVAILABLE: "Unavailable"
                }
            },
            CROWDSALE: {
                TITLE: "Deploy crowdsale"
            },
            MINT: {
                TITLE: "Mint tokens",
                AMOUNT: {
                    LABEL: "Amount",
                    ERROR: "Invalid amount",
                    PLACEHOLDER: "Enter amount of tokens to mint"
                }
            },
            BURN: {
                TITLE: "Burn tokens"
            },
            PAYMENT_WALLET: {
                LABEL: "Pay from",
                ERROR: "Invalid wallet",
                PLACEHOLDER: "Choose which wallet to charge"
            },
            ACTIONS: {
                DEPLOY: "Deploy",
                MINT: "Mint",
                BURN: "Burn",
                DEPLOY_CROWDSALE: "Deploy"
            },
            MESSAGES: {
                INVALID_FORM: "Invalid form values",
                SUCCESS: "Successfully updated token",
                INITIATED: "Deployment initiated",
                MINT_SUCCESS: "Successfully minted",
                BURN_SUCCESS: "Successfully burned",
                TRANSACTION_INITIATED: "Transaction initiated. Please, wait...",
                CROWDSALE_DEPLOY_SUCCESS: "Crowdsale contract successfully deployed",
                STAGE_DEPLOY_SUCCESS: "Stage successfully deployed"
            },
            STAGE: {
                TITLE: "Deploy stage"
            },
            DEPLOY: {
                TITLE: "Deploy token",
                FEE: "Transaction fee",
                PRICE: "Transaction price"
            }
        },
        DISTRIBUTION: {
            INVALID_FORM: "Invalid distribution. Check values",
            MESSAGES: {
                SUCCESS: "Distribution was successfully updated",
                STAGE_SUCCESS: 'Stages were successfully updated',
                STAGE_FAILED: 'The sum of tokens in the stages of the project exceeds the total supply of tokens.'
            },
            EXCEEDED_LIMIT: "Too many rows. Current limit is 15",
            INVALID_TABLE: "The sum of tokens in the stages of the project exceeds the total supply of tokens."
        },
        STAKING_MODAL: {
            TITLE: 'Confirm stake',
            CONTENT: 'Please, confirm stake of',
            DONE: 'Done'
        },
        STAKING_WITHDRAW_MODAL: {
            TITLE: 'Finish staking',
            CONTENT: 'You are withdrawing from staking. Withdrawing from staking will move all tokens to your wallet and disable you from next round of reward. Are you sure?',
            DONE: 'Done',
        },
        STAKING_REWARD_MODAL: {
            TITLE: 'Claim your reward',
            CONTENT: 'You will receive the approximate reward of ',
            DONE: 'Done'
        },
        MY_OPERATIONS: {
            TITLE: "Operations",
            ACTIONS: {
                REFRESH: "Refresh"
            },
            NO_ITEMS: "There are no operations",
            TABLE: {
                HASH: "Tx Hash",
                TYPE: "Type",
                STATE: "State",
                CREATED_AT: "Created at",
                FINISHED_AT: "Finished at"
            }
        }
    }
};

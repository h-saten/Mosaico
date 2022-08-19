export const locale = {
  lang: 'en',
  data: {
    COMING_SOON: {
      TITLE: 'Coming soon',
      INFO_BELOW: "We are working on it"
    },
    WALLET_PAYMENT: {
      LABEL: "Pay with",
      LABEL_DAO: "Pay with DAO",
      PLACEHOLDER: "Choose the wallet that will be charged",
      ERROR: "Invalid wallet",
      HINT: "We will execute transaction on your behalf",
      CURRENT_BALANCE: "Available",
      DEPLOY: {
        PRICE: "Transaction cost",
        FEE: "Transaction fee"
      }
    },
    NEW_TOKEN: {
      FORM: {
        NAME: {
          LABEL: 'Token name',
          PLACEHOLDER: 'e.g Mosaico, Sapiency',
          ERROR: 'Name is invalid',
          LINK: 'https://docs.mosaico.ai/fundamentals/tokens./what-is-a-token-name'
        },
        SYMBOL: {
          LABEL: 'Token symbol',
          PLACEHOLDER: 'e.g USDT, MOS or SPCY',
          ERROR: 'Symbol is invalid',
          LINK: 'https://docs.mosaico.ai/fundamentals/tokens./what-is-a-token-symbol'
        },
        DEC: {
          LABEL: 'Decimals',
          PLACEHOLDER: 'Enter the the number of digits',
          ERROR: 'Field is invalid'
        },
        NETWORK: {
          LABEL: 'Blockchain',
          PLACEHOLDER: 'Choose the network to deploy token on',
          ERROR: 'You have to choose blockchain'
        },
        SUPP: {
          LABEL: 'Initial supply',
          PLACEHOLDER: 'Enter how many tokens to emit',
          ERROR: 'Enter valid number',
          LINK: 'https://docs.mosaico.ai/fundamentals/marketplace/what-is-a-initial-supply'
        },
        TYPE: {
          LABEL: 'Token type',
          PLACEHOLDER: '',
          ERROR: 'Choose token type'
        },
        MINTABLE: {
          LABEL: "Mintable",
          LINK: 'https://docs.mosaico.ai/fundamentals/mint-vs.-burn./what-is-mintable-mint'
        },
        BURNABLE: {
          LABEL: "Burnable",
          LINK: 'https://docs.mosaico.ai/fundamentals/mint-vs.-burn./what-is-burnable-burn'
        },
        GOVERNANCE: {
          LABEL: "Governance"
        },
        WALLET: {
          LABEL: "Payment method",
          PLACEHOLDER: "Choose wallet",
          ERROR: "Invalid wallet",
          HINT: "Choose the wallet you want to pay with for transaction"
        },
      },
      GAS_ESTIMATE: "Transaction price",
      TITLES: {
        TOKEN_TYPE: "Choose token type",
        TOKEN_DETAILS: "Enter token details",
        TOKEN_PAYMENT_METHOD: "Choose payment method"
      },
      FEE: "Fee",
      TOKEN_TYPES: {
        UTILITY: 'Utility',
        UTILITY_HINT: 'An utility token is a crypto token that serves some use case within a specific ecosystem and can be exchanged to some goods.',
        UTILITY_LINK: 'https://docs.mosaico.ai/fundamentals/tokens./what-is-a-utility-token',
        SECURITY: 'Security',
        SECURITY_HINT: 'Security tokens represent ownership shares in a company that does business using blockchain technology.',
        SECURITY_LINK: 'https://docs.mosaico.ai/fundamentals/tokens./what-is-a-security-tokens',
        GOVERNANCE: "Governance",
        GOVERNANCE_HINT: "Governance tokens allow hodlers to actively participate in the life of DAO, create proposals, and affect company direction via voting",
        GOVERNANCE_LINK: 'https://docs.mosaico.ai/fundamentals/tokens./what-is-a-governance-token'
      },
      ACTIONS: {
        SUBMIT: 'Create token',
        CANCEL: 'Cancel',
        NEXT: 'Next',
        SUBMITTING: 'Deploying',
        BACK: 'Back to type'
      },
      MESSAGES: {
        INVALID_FORM: 'Form has invalid values. Review the form again to continue.',
        SUCCESSFULLY_CREATED: 'Token was successfully created'
      }
    },
    IMPORT_TOKEN: {
      FORM: {
        CONTRACT_ADDRESS: {
          LABEL: 'Contract address',
          PLACEHOLDER: 'e.g 0x0f152296df89a7c7e904764523739f9d6f48fed9',
          ERROR: 'Invalid address'
        }
      },
      ACTIONS: {
        SUBMIT: 'Import token',
        SUBMITTING: 'Importing',
      },
      MESSAGES: {
        SUCCESSFULLY_CREATED: 'Token was successfully imported',
        IMPORT_ERROR_TITLE: 'Failed to retrieve token data',
        IMPORT_ERROR_CONTENT: 'An invalid contract address was provided or the token source code was not verified.',
        CANNOT_IMPORT_TITLE: 'Cannot import token',
        CANNOT_IMPORT_CONTENT: 'The token already exists in the system or is not a valid ERC20 token.'
      }
    },
    INVALID_TOKEN_NAME: "Invalid token name",
    INVALID_TOKEN_SYMBOL: "Invalid token symbol",
    INVALID_NETWORK: "Invalid network",
    INVALID_TYPE: "Given token type is not yet supported",
    INVALID_DECIMALS: "Invalid decimals",
    INVALID_COMPANY_ID: "Invalid company id",
    TOKEN_ALREADY_EXISTS: "Token already exists",
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
        INFO_SUBSCRIBED_TO_NEWSLETTER_NOT_LOGGED_IN: "You successfully signed up for newsletter",
        INFO_UNSUBSCRIBED_TO_NEWSLETTER: "Unsubscribed from the newsletter",
        INFO_CONFIRM_UNSUBSCRIBED: "Do you want to unsubscribe from the newsletter?",
      }
    },
    USER_ALREADY_SUBSRIBED_TO_NEWSLETTER: 'Already subscribed to the newsletter',
    PROJECT: {
      LIKE: {
          UNAUTHORIZED: "You have to sign in to like the project."
      },
      FEATURED: 'Featured project',
      ACTIONS: {
          INVEST_NOW: 'Invest now',
          VIEW_DETAILS: 'View details',
          LEARN_MORE: 'Learn more',
          DETAILS_SOON: 'View details',
          DETAILS_SOON_HINT: 'We are migrating projects from previous version and details will be available soon.'
      }
    },
    FOLLOW_US: 'follow us',
    JOIN_OUR_NEWSLETTER: {
      TITLE: 'Newsletter',
      SUBSCRIBE: 'SUBSCRIBE TO OUR',
      PLACEHOLDER: 'Enter your email address',
      SEND: 'Send',
      SUBMIT: 'By submitting your email address, you agree to Mosaico <a href="https://v1.mosaico.ai/assets/pdf/Regulamin_platformy_MOSAICO_11_2019.pdf" target="_blank" class="white-text">Terms and Conditions</a> and <a href="https://v1.mosaico.ai/assets/pdf/Polityka_prywatnosci_09_2020.pdf" target="_blank" class="white-text">Privacy Policy</a>.',
      SUBSCRIBING: 'Subscribe to our newsletter to get access to pre sale with preferable prices of Tokens in upcoming ICOs!'
    }
  }
};

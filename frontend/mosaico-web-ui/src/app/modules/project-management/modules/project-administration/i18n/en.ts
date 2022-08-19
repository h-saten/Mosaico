export const locale = {
    lang: 'en',
    data: {
      "investors.certificate_generator.background_info": "The background dimension must be 1528x1080. The maximum file size is 3MB.",
      "investors.certificate_generator.bold": "Bold font",
      "investors.certificate_generator.center": "Center",
      "investors.certificate_generator.center_element": "Center the item",
      "investors.certificate_generator.date": "Date",
      "investors.certificate_generator.font_size": "Font size",
      "investors.certificate_generator.height": "Height",
      "investors.certificate_generator.info": "All numerical values have been scaled to match the editor's resolution.",
      "investors.certificate_generator.left": "Left",
      "investors.certificate_generator.left_spacing": "Left distance",
      "investors.certificate_generator.name": "Name",
      "investors.certificate_generator.number": "Number",
      "investors.certificate_generator.on": "Active?",
      "investors.certificate_generator.pdf_generator": "Generate PDF preview",
      "investors.certificate_generator.right": "Right",
      "investors.certificate_generator.rounded_corners": "Rounded corners",
      "investors.certificate_generator.save_background": "Save the background",
      "investors.certificate_generator.save_configuration": "Save the configurations",
      "investors.certificate_generator.scale": "Scale:",
      "investors.certificate_generator.text_alignment": "Text alignment",
      "investors.certificate_generator.text_color": "Text color",
      "investors.certificate_generator.title": "Investor certificate generator",
      "investors.certificate_generator.token_shortcut": "Attach the token shortcut to the amount",
      "investors.certificate_generator.tokens": "Number of tokens",
      "investors.certificate_generator.upper_spacing": "Top distance",
      "investors.certificate_generator.view_module": "Preview mode",
      "investors.certificate_generator.width": "Width",

      "issuer.certificate.certificate_preview": "open the certificate overview",
      "issuer.certificate.default_currency_amount_field": "Default currency amount in the purchase form",
      "issuer.certificate.description": "The certificate is sent to every investor who buys tokens. The investor receives a certificate to his email address and always has access to the current version of the certificate in his portfolio on the mosaico platform.",
      "issuer.certificate.send_certificate_field": "Sending certificates to investors",
      "issuer.certificate.title": "Certificate settings",
      "issuer.certificate.invalid_img": "Invalid image",
      "issuer.certificate.invalid_img_resolution": "Invalid image resolution",
      "issuer.certificate_generator.alert": "Certificate configuration changed.",
      "FORBIDDEN_TO_CLAIM_AIRDROP": "AirDrop is inactive. Cannot claim",
      "FILTER_FORM": {
        "APPLY": "Apply",
        "TIME_RANGE": "Time Range",
        "CLEAR": "Clear",
        "CHOOSE_TIMEFRAME": "Choose timeframe",
        "CHOOSE_STATUSES": "Choose statuses",
        "CHOOSE_PAYMENT_METHODS": "Choose payment methods",
      },
      "TRANSACTIONS": {
        "ACTIONS": {
          "REFRESH": "Refresh",
          "FILTERS": "Filters",
          "EXPORT_CSV": "Export CSV"
        }
      },
      "DASHBOARD": {
        "VISITS_CHART": {
          "title": "Token page visits",
          "token_page_label": "Token page visits",
          "fund_page_label": "Fund page visits",
        },
        "SUMMARY": {
          "token_page_visits": "Visits",
          "fund_page_visits": "Visits on purchase page",
          "transactions_number": "Number of transactions",
          "followers": "Following",
          "transaction_avg": "Average transaction",
          "transaction_min": "Smallest purchase amount",
          "transaction_max": "Highest purchase amount",
          "transaction_median": "Median purchase amount",
        },
        "SALE_STATISTICS": {
          "title": "Sale statistics"
        },
        "TOP_INVESTORS": {
          "title": "Top investors"
        },
        "DAILY_CAPITAL": {
          "title": "Daily sale statistics",
          "chart_label": "Raised funds",
          "prev_month": "previous month",
          "next_month": "next month"
        },
        "FUNDS_RAISED": {
          "title": "Raised funds"
        }
      },
      PAGE_INTRO:{
        PAGE_FORM_Title:"Introduction Video",
        PAGE_FORM_VIDEO_URL:"Video URL",
        PAGE_FORM_UPLOAD_VIDEO:"Upload Video",
        PAGE_FORM_DISAPLY_UPLOADED_VIDEO:"Display Uploaded Video",
        PAGE_BUTTON:{
          SAVE:"Save",
          SHOW:"Show",
          UPLOAD:"Upload"
        },
        MESSAGE:{
          VALIDATION:"Please upload video or video URL.",
          FILE_UPLOAD:"Please upload intro video.",
          FILE_EXTENTION:"you can only upload .mp4 file format.",
          FILE_SIZE:"File size should be less than 100MB.",
          VIDEO_LINK:"Please insert valid URL",
          SUCCESS:"Changes saved successfully"
        }
      },
      PROJECT_AIRDROPS: {
        COLUMN_TITLES: {
          CAMPAIGN: "Campaign",
          STARTED_AT: "Started at",
          FINISHED_AT: "Finished at",
          PROGRESS: "Progress",
          ACTIONS: "Actions"
        },
        PROGRESS_COLUMN: {
          INCLUDED_IN_SALE: "Included in sale",
          TOKENS_PER_PERSON: "Tokens per person" 
        },
        ACTION_COLUMN: {
          COPY_LINK: "Copy link",
          PAY_AIRDROP: "Pay airdrop",
          STOP_CAMPAIGN: "Stop campaign",
          CREATE_AIRDROP: "Create Airdrop"
        },
        ALERTS: {
          AIRDROP_WAS_SUSPENDED: "Airdrop was suspended",
          URL_COPIED: "URL copied",
          SUCCESS_MSG: "Participants successfully imported",
          UNHANDLED_ERROR: "File format is invalid. We support following formats: XYZ",
          AIRDROP_EXHAUSTED: "Airdrop supply has finished."
        },
        STOP_CAMPAIGN_FORM: {
          CONFIRM_YOUR_ACTION: "Confirm your action",
          SURE_TO_STOP: "Are you sure you want to stop campaign? no one will be able to withdraw the reward anymroe",
          ACTIONS: {
            SUBMIT: "Submit",
            CANCEL: "Cancel"
          }
        },
        PAY_AIRDROP: {
          TITLE: 'Withdraw tokens',
          DESCRIPTION: 'You are about to distribute <span class="fw-bold">{{pendingTokens}} {{tokenSymbol}}</span> tokens among <span class="fw-bold">{{pendingUsersCount}}</span> users.',
          ACTION: 'Start'
        },
        DETAILS: {
          CLAIMED_AT: "Claimed at",
          WITHDRAWAL_AT: "Withdrawal at",
          TOKEN: "Token",
          TRANSACTION: "Transaction",
          IMPORT: "Import"
        }
      },
      CREATE_AIRDROP: {
        FORM: {
          TITLE: "Create Airdrop",
          LABELS: {
            NAME: "Name",
            START_DATE: "Start date",
            END_DATE: "End date",
            TOTAL_TOKEN: "Total token supply",
            MAX_WITHDRAW: "Max individual withdraw"
          },
          RADIO_BUTTONS: {
            PUBLIC: "Public",
            PURCHASE_COUNT: "Count as purchase",
          },
          ACTIONS: {
            CREATE: "Create"
          },
          PLACEHOLDERS: {
            CHOOSE_NAME: "Choose the name for campaign",
            CHOOSE_TIME: "Choose timeframe",
            ENTER_AMOUNT: "Enter amount of tokens you want to distribute",
            ENTER_TOKENS_AMOUNT: "Enter amount of tokens you want to distribute"
          },
          VALIDATORS: {
            INVALID_VALUES: "Form has invalid values.",
            INVALID_CAMPAIGN_NAME: "Campaign name is invalid",
            INVALID_TOKEN_SUPPLY: "Invalid token supply",
            INVALID_TOKEN_AMOUNT: "Invalid token amount"
          }
        }
      },
      FEE: {
        TITLE: {
          TOTAL_FEE: "Total fee:",
          TRANSACTION: "Transaction:"
        },
        WITHDRAW: {
          WITHDRAW_FEE: "Withdraw Fee",
          WALLET: "Wallet",
          ENTER_WALLET_ADDRESS: "Enter wallet address",
          CONFIRM: "Confirm"
        }
      },
      AFFILIATION: {
        ERR_MSG: {
          USER_NOT_FOUND: "We couldn't find such user in our database",
          AFFILIATION_DISABLED: "Affiliation disabled"
        }
      }
    }
};

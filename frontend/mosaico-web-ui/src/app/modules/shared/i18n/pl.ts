export const locale = {
  lang: 'pl',
  data: {
    COMING_SOON: {
      TITLE: 'Już wkrótce',
      INFO_BELOW: "Jeszcze nad tym pracujemy"
    },
    WALLET_PAYMENT: {
      LABEL: "Zapłać przy użyciu",
      LABEL_DAO: "Zapłać jako DAO",
      PLACEHOLDER: "Wybierz portfel z którego zostaną pobrane środki",
      ERROR: "Nieprawidłowy portfel",
      HINT: "Zajmiemy się wykonaniem transakcji w Twoim imieniu",
      CURRENT_BALANCE: "Dostępne",
      DEPLOY: {
        PRICE: "Koszt transakcji",
        FEE: "Opłata transakcyjna"
      }
    },
    NEW_TOKEN: {
      FORM: {
        NAME: {
          LABEL: 'Nazwa tokena',
          PLACEHOLDER: 'e.g Mosaico, Sapiency',
          ERROR: 'Niepoprawna nazwa',
          LINK: 'https://docs.mosaico.ai/podstawowe-pojecia/tokeny./czym-jest-nazwa-tokena'
        },
        SYMBOL: {
          LABEL: 'Symbol tokena',
          PLACEHOLDER: 'e.g USDT, MOS czy SPCY',
          ERROR: 'Niepoprawny symbol',
          LINK: 'https://docs.mosaico.ai/podstawowe-pojecia/tokeny./czym-jest-symbol-tokena'
        },
        DEC: {
          LABEL: 'Części dziesiętne',
          PLACEHOLDER: 'Podaj ilość cyfr',
          ERROR: 'Niepoprawne wartość'
        },
        NETWORK: {
          LABEL: 'Blockchain',
          PLACEHOLDER: 'Wybierz sieć dla utworzenia tokena',
          ERROR: 'Musisz wybrać jedną z obsługiwanych sieci'
        },
        SUPP: {
          LABEL: 'Początkowa podaż',
          PLACEHOLDER: 'Wprowadz ilość tokenów do utworzenia',
          ERROR: 'Wprowadź poprawną liczbę',
          LINK: 'https://docs.mosaico.ai/podstawowe-pojecia/rynek./czym-jest-poczatkowa-podaz'
        },
        TYPE: {
          LABEL: 'Typ tokena',
          PLACEHOLDER: '',
          ERROR: 'Wybierz typ tokena'
        },
        MINTABLE: {
          LABEL: "Mintable",
          LINK: 'https://docs.mosaico.ai/podstawowe-pojecia/mint-vs.-burn./czym-jest-mintable-mint'
        },
        BURNABLE: {
          LABEL: "Burnable",
          LINK: 'https://docs.mosaico.ai/podstawowe-pojecia/mint-vs.-burn./czym-jest-burnable-burn'
        },
        GOVERNANCE: {
          LABEL: "Governance"
        },
        WALLET: {
          LABEL: "Metoda płatności>",
          PLACEHOLDER: "Wybierz portfel",
          ERROR: "Niepoprawny portfel",
          HINT: "Wybierz portfel z którego zostaną pobrane środki"
        },
      },
      GAS_ESTIMATE: "Koszt transakcji",
      TITLES: {
        TOKEN_TYPE: "Wybierz typ tokena",
        TOKEN_DETAILS: "Wprowadź szczegóły tokena",
        TOKEN_PAYMENT_METHOD: "Wybierz metodę płatności"
      },
      FEE: "Opłata",
      TOKEN_TYPES: {
        UTILITY: 'Utility',
        UTILITY_HINT: 'Token użytkowy ma wartość w ramach ecosystemu, którego jest częścią. Najczęściej pełni funkcję środka wymiany na inne dobra.',
        UTILITY_LINK: 'https://docs.mosaico.ai/podstawowe-pojecia/tokeny./czym-jest-tokeny-uzytkowe',
        SECURITY: 'Security',
        SECURITY_HINT: 'Reprezentuje prawo własności akcji przedsiębiorstwa działającego w opraciu o sieć blockchain.',
        SECURITY_LINK: 'https://docs.mosaico.ai/podstawowe-pojecia/tokeny./czym-jest-tokeny-security',
        GOVERNANCE: "Governance",
        GOVERNANCE_HINT: "Jego posiadacze uprawnieni są do czynnego udziału w ramach działań prowadzonych przez DAO, zarówno poprzez tworzenie swoich propozycji jak i głosowania na istniejące propozycje kierunku jego rozwoju.",
        GOVERNANCE_LINK: 'https://docs.mosaico.ai/podstawowe-pojecia/tokeny./czym-jest-tokeny-governance'
      },
      ACTIONS: {
        SUBMIT: 'Utwórz token',
        CANCEL: 'Anuluj',
        NEXT: 'Dalej',
        SUBMITTING: 'Tworzenie',
        BACK: 'Wróć do wyboru typu'
      },
      MESSAGES: {
        INVALID_FORM: 'Dane formularza są niepoprawne. Popraw je, aby kontynuować.',
        SUCCESSFULLY_CREATED: 'Token został poprawnie utworzony'
      }
    },
    IMPORT_TOKEN: {
      FORM: {
        CONTRACT_ADDRESS: {
          LABEL: 'Adres kontraktu',
          PLACEHOLDER: 'np. 0x0f152296df89a7c7e904764523739f9d6f48fed9',
          ERROR: 'Niepoprawny adres'
        }
      },
      ACTIONS: {
        SUBMIT: 'Importuj token',
        SUBMITTING: 'Importowanie',
      },
      MESSAGES: {
        SUCCESSFULLY_CREATED: 'Token został poprawnie zaimportowany',
        IMPORT_ERROR_TITLE: 'Nie udało się pobrać danych tokena',
        IMPORT_ERROR_CONTENT: 'Podano niepoprawny adres kontraktu lub kod źródłowy tokena nie został zweryfikowany.',
        CANNOT_IMPORT_TITLE: 'Import tokena jest niemożliwy',
        CANNOT_IMPORT_CONTENT: 'Token już istnieje w systemie lub nie jest prawidłowym tokenem ERC20.'
      }
    },
    INVALID_TOKEN_NAME: "Niepoprawna nazwa tokena",
    INVALID_TOKEN_SYMBOL: "Niepoprawny symbol tokena",
    INVALID_NETWORK: "Niepoprawna sieć",
    INVALID_TYPE: "Typ tokena nie jest wspierany.",
    INVALID_DECIMALS: "Niepoprawna wartość dziesiętna",
    INVALID_COMPANY_ID: "Niepoprawny identyfikator firmy",
    TOKEN_ALREADY_EXISTS: "Token już istnieje",
    SUBSCRIPTION_TO_NEWSLETTER: {
      TITLE: "Zapisz się na alert dla inwestorów",
      ACTIONS: {
        SAVE: "Zapisz się",
        CANCEL: 'Anuluj',
        SIGN_ME_OUT: 'Wyloguj mnie',
        // ADD: 'Add logo',
        // CHANGE: 'Edytuj logo'
      },
      MESSAGES: {
        INFO: "Chcesz dowiedzieć się więcej o projekcie? Być jednym z pierwszych inwestorów? Zapisz się na naszą białą listę!",
        INFO_FOR_NOT_LOGGED_IN: "Na razie tylko dla zalogowanych!",
        INFO_FOR_NOT_LOGGED_IN2: "Zaloguj sie",
        INFO_ALREADY_SUBSCRIBED: "Zapisałeś się już do newslettera projektu.",
        INFO_SUBSCRIBED_TO_NEWSLETTER_LOGGED_IN: "Zapisałem się do newslettera!",
        INFO_SUBSCRIBED_TO_NEWSLETTER_NOT_LOGGED_IN: "Odbierz od nas wiadomość i kliknij w link, aby potwierdzić chęć zapisania się do newslettera.",
        INFO_UNSUBSCRIBED_TO_NEWSLETTER: "Rezygnacja z subskrypcji biuletynu",
        INFO_CONFIRM_UNSUBSCRIBED: "Chcesz zrezygnować z otrzymywania newslettera?",
      }
    },
    USER_ALREADY_SUBSRIBED_TO_NEWSLETTER: 'Już zapisałeś się do newslettera',
    PROJECT: {
      LIKE: {
          UNAUTHORIZED: "Musisz się zalogować, aby polubić ten projekt."
      },
      FEATURED: 'Polecany projekt',
      ACTIONS: {
          INVEST_NOW: 'Zainwestuj',
          VIEW_DETAILS: 'Zobacz więcej',
          LEARN_MORE: 'Więcej info',
          DETAILS_SOON: 'Zobacz więcej',
          DETAILS_SOON_HINT: 'Migrujemy projekty z poprzedniej wersji, a szczegóły będą dostępne wkrótce.'
      }
    },
    FOLLOW_US: 'śledź nas na mediach społecznosciowych',
    JOIN_OUR_NEWSLETTER: {
      TITLE: 'Newslettera',
      SUBSCRIBE: 'Zapisz się do naszego',
      PLACEHOLDER: 'Wpisz swój adres e-mail',
      SEND: 'Wyślij',
      SUBMIT: 'Przesyłając swój adres e-mail, wyrażasz zgodę na <a href="https://v1.mosaico.ai/assets/pdf/Regulamin_platformy_MOSAICO_11_2019.pdf" target="_blank" class="white-text">Regulamin</a> i <a href="https://v1.mosaico.ai/assets/pdf/Polityka_prywatnosci_09_2020.pdf" target="_blank" class="white-text">Politykę Prywatności </a>Mosaico.',
      SUBSCRIBING: 'Zapisz się do naszego newslettera, aby uzyskać dostęp do przedsprzedaży z preferencyjnymi cenami Tokenów w nadchodzących ICO!'
    }
  }
};

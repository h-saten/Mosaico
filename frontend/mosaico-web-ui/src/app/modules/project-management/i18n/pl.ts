export const locale = {
    lang: 'pl',
    data: {
        HEADER: {
            BUTTON_FOLLOW_US: "Obserwuj",
            BUTTON_NO_FOLLOW_US: "Nie obserwuj",
            BUTTON_SHARE: "Udostępnij"
        },
        FOOTER: {
          CONTACTS: "Kontakt",
          DESCRIPTION: "O firmie",
      },
        MESSAGE: {
            SUCCESS: "Sukces!",
            PARTNER_DELETED: "Partner został pomyślnie usunięty!"
        },
        NEW_PROJECT: {
            TITLE: "Utwórz projekt",
            FORM: {
                TITLE: {
                    LABEL: "Nazwa Projektu",
                    PLACEHOLDER: "Nazwij swój projekt",
                    ERROR: "Nieprawidłowa nazwa projektu",
                    ERROR_DUPLICATE: 'Taki projekt już istnieje'
                },
                SLUG: {
                    LABEL: "Ślimak",
                    PLACEHOLDER: "Wpisz ślimak",
                    ERROR: "Nieprawidłowy ślimak",
                    ERROR_DUPLICATE: 'Wybierz inny ślimak'
                },
                COMPANY: {
                    LABEL: 'DAO',
                    PLACEHOLDER: 'Wybierać DAO',
                    ERROR: 'DAO jest wymagane'
                },
                DESCRIPTION: {
                    LABEL: "Opis",
                    PLACEHOLDER: "Opisz krótko, czego dotyczy Twój projekt",
                    ERROR: "Wpisz poprawny opis"
                },
                ACTIONS: {
                    CONTINUE: "Kontyntynuj",
                    CREATE_DAO: 'Tworzyć DAO'
                }
            },
            MESSAGES: {
                INVALID_FORM: 'Formularz ma nieprawidłowe wartości. Proszę, napraw je, aby kontynuować',
                SUCCESSFULLY_CREATED: 'Projekt pomyślnie utworzony',
                NO_DAO: 'Nie jesteś częścią żadnego DAO. Utwórz jako pierwszy!'
            }
        },
        PROJECT: {
            'counter.days': "Dni",
            'counter.hours': "Godziny",
            'counter.minutes': "Minuty",
            'counter.seconds': "Sekundy",
            'info.col.start': 'Start',
            'info.col.end': 'Koniec',
            'info.col.blockchain': 'Blockchain',
            'info.col.token_price': 'Cena tokena',
            'bar.title': 'Postęp emisji ',
            'bar.col.1': 'Liczba kupujących',
            'bar.col.2': 'Sprzedano',
            'bar.col.3': 'Soft cap',
            'bar.col.4': 'Hard cap',
            'bar.tooltip.softcap': `Softcap odnosi się do minimalnego zdefiniowanego limitu zbierania funduszy określonego przez zespół projektu w celu jego pozyskiwania`,
            'menu.overview': 'Przegląd',
            'menu.about': 'O projekcie',
            'menu.packages': 'Pakiety inwestycyjne',
            'menu.news': 'Press Room',
            'menu.faq': 'FAQ',
            'menu.feedback': "Opinie inwestorów",
            "faq.title": "Często zadawane pytania (FAQ)",
            "feedback.title": "Opinie inwestorów",
            // "faq.title" : "FAQ",
            "packages.title": 'Wybierz pakiet inwestycji',
            "packages.btn.select": "Wybierz plan",
            "manage.section": "Zarządzaj sekcją",
            "show_more.name.articles": "wiadomości",
            PAYMENT_METHOD:{
                TITLE: 'Metoda płatności',
                UPDATE:{
                    SUCCESS:'Zaktualizowano metodę płatności.',
                },
                BANK_DETAILS:{
                    NOT_ADDED: 'Nie podano danych bankowych, zaktualizuj dane przelewu bankowego.'
                }
            },
            PAYMENT_CURRENCY:{
                TITLE: 'Waluta płatności'
            },
            NFT: {
                TITLE: "Kolekcja NFT"
            }
        },
        EDIT_PROJECT: {
            TITLE: "Członkowie projektu",
            COLUMNS: {
                 STATUS: "Status",
                 ROLE: "Rola",
                 ACTIONS: 'Działania'
            },
            ACTIONS: {
                 ADD: "Dodaj nowego członka",
                 REMOVE: "Usuń"
            },
            ROLE: {
                 OWNER: "Właściciel",
                 MEMBER: "Członek"
            },
            STATUS: {
                 ACCEPTED: "Zaakceptowano",
                 WAITING: "Oczekuje"
            },
            REMOVE_FORM: {
                 TITLE: "Potwierdź działanie",
                 DESCRIPTION: "Czy na pewno chcesz usunąć użytkowników z projektu?",
                 ACTIONS: {
                     SUBMIT: "Wyślij",
                     CANCEL: "Anuluj"
                 }
            },
            ALERTS: {
                 REGULAR_VALIDATOR: "Nieprawidłowy email użytkownika",
                 SUCCESSFULLLY_ADDED: "Użytkownik został pomyślnie dodany",
                 SUCCESSFULLY_REMOVED: "Pomyślnie usunięto członka zespołu",
                 SUCCESSFULLY_UPDATED: "Pomyślnie zaktualizowano członka zespołu"
            }
         },
        FORM: {
            "lbl.name": "Imię",
            "lbl.role": "Rola",
            "lbl.facebook": "Facebook",
            "lbl.twitter": "Twitter",
            "lbl.linkedin": "Linkedin",
            "url.invalid": "nieprawidłowy URL (wzór: https://example.com)",
            "allowed_file": "Dozwolone formaty plików: JPEG, JPG, PNG, GIF",
            "move.down": "przesuń się raz w dół",
            "move.up": "przesuń się raz w górę",
            "edit": "Edytować",
            "delete": "Usuń",
            CHANGE_AVATAR : 'Zmień avatar',
            CANCEL_AVATAR : 'Anuluj',
            REMOVE_AVATAR : 'Usuń avatar'
        },
        PROJECT_TEAM: {
            TITLE: "Zespół",
            SUBTITLE: "Dodaj osoby, które współtworzą projekt, daj znać inwestorowi.",
            EMAIL_ENTER: "Wpisz adres email użytkownika",
            BTN_SAVE: "Zapisz",
            BTN_CREATE: "Tworzyć",
            BTN_CLOSE: "Zamknij",
            BTN_CANCEL: "Anuluj",
            SAVED_SUCCESSFULLY: "Dane członka zespołu zostały pomyślnie zapisane.",
            ADD: "Dodaj członka zespołu",
            EDIT: 'Zmień dane członka zespołu',
            DELETE: 'Usuń członka zespołu',
            DELETED: 'Usunięto członka zespołu',
            MOVE_LEFT: 'Przesuń w lewo',
            MOVE_RIGHT: 'Przesuń w prawo',
            INVALID_NAME: 'Nieprawidłowa nazwa członka',
            INVALID_ROLE: 'Nieprawidłowa rola członka'
        },
        PROJECT_PARTNER: {
            TITLE: "Partnerzy",
            SUBTITLE: "Zarządzaj ludźmi, którzy współpracują z projektem, daj znać swojemu inwestorowi.",
            BTN_SAVE: "Zapisz",
            BTN_CLOSE: "Zamknij",
            BTN_CANCEL: "Anuluj",
            SAVED_SUCCESSFULLY: "Dane partnera zostały pomyślnie zapisane.",
            ADD: "Dodaj nowego partnera",
            EDIT: 'Zmień dane partnera',
            DELETE: 'Usuń partnera',
            MOVE_LEFT: 'Przesuń w lewo',
            MOVE_RIGHT: 'Przesuń w prawo',
            INVALID_NAME: 'Nieprawidłowa nazwa partnera',
            INVALID_ROLE: 'Nieprawidłowa rola partnera'
        },
        PROJECT_OVERVIEW: {
            FORM: {
                DESCRIPTION: {
                    LABEL: "Opis Projektu",
                    PLACEHOLDER: "Opisz krótko, czego dotyczy Twój projekt",
                    ERROR: "Wpisz poprawny opis"
                },
                TOKEN: {
                    LABEL: "Wybierz token"
                },
                ABOUT: {
                    LABEL: "O projekcie",
                    PLACEHOLDER: "Opisz krótko, czego dotyczy Twój projekt",
                    ERROR: "Wpisz poprawny opis",
                    ACTIONS: {
                        SAVE: "Zapisz",
                        CANCEL: "Anuluj"
                    }
                }
            },
            NO_TOKENOMICS_AVAILABLE: "Tokenomika jest niedostępna",
            EXCHANGES: {
                NO_EXCHANGES: "Dla tego projektu nie skonfigurowano żadnej giełdy",
                NO_TOKEN: 'Nie można wczytać giełd. Musisz najpierw wybrać token',
                TITLE: 'Giełda',
                MESSAGES: {
                    ACTIVATED: "Wymiana została pomyślnie aktywowana",
                    DEACTIVATED: "Wymiana została pomyślnie dezaktywowana"
                }
            },
            FEATURES: {
                TITLE: "Cechy",
                POS: "POS",
                POS_HINT: "The mechanism to receive a reward in exchange for frozen tokens. The reward is generated on a periodical basis and split between all stakers proportionally to the amount of owned frozen tokens. The reward can be represented by USDT or other token and will be deposit directly to user wallet.",
                VESTING: "Vesting",
                VESTING_HINT: "Vesting is the process of locking and releasing tokens after a given time. Just like in traditional finance, vesting in the crypto world is often used to ensure long-term commitment to a project from team members.",
                MINT: "Mint",
                MINT_HINT: "Generation of new tokens",
                BURN: "Burn",
                BURN_HINT: "The act of burning tokens effectively removes tokens from the available supply, which decreases the number in circulation.",
                INVESTMENT_PACKAGES: "Pakiety inwestycyjne",
                INVESTMENT_PACKAGES_HINT: "Amount of tokens offered in preferable price or with additional asset as service or product of and Investee.",
                DEFLATION: "Deflacja",
                DEFLATION_HINT: "Deflation is the processes of burnining tokens during some operation. For example, during a transaction some % of tokens can be automatically burned",
                STATES: {
                    ENABLED: "Włączony",
                    DISABLED: "Wyłączony",
                    AVAILABLE: "Dostępny",
                    UNAVAILABLE: "Niedostępne"
                }
            },
            VIEW_DETAILS: "Szczegóły",
            SALES_INFO: {
                TITLE: "Informacje o sprzedaży",
                TOKEN_PRICE: "Cena tokena",
                MIN_INVEST: "Min. inwestycja",
                MAX_INVEST: "Maks. inwestycja",
                SOFT_CAP: "Softcap",
                HARD_CAP: "Hardcap",
                TOTAL_SUPPLY: "Całkowita podaż"
            },
            TOKENOMICS: {
                TITLE: "Tokenomika"
            },
            SIDEBAR: {
                ADDRESS: "Adres",
                ADDRESS_COPIED_MESSAGE: 'Adres skopiowany!',
                INVESTOR_CERTIFICATE: 'Certyfikat inwestora',
                INVESTOR_CERTIFICATE_LOADING: 'Trwa przygotowywanie Twojego certyfikatu'
            },
            STAGES: {
                TITLE: "Etapy projektu",
                TABLE: {
                    STAGE: "Scena",
                    START_DATE: "Data rozpoczęcia",
                    END_DATE: "Data zakonczenia",
                    TOKEN_PRICE: "Cena tokena",
                    TOKEN_SUPPLY: "Ilość tokenów",
                    NO_STAGES: "Etapy nie są skonfigurowane"
                }
            },
            ACTIONS: {
                NEW_TOKEN: "Utwórz nowy token",
                EDIT: "Edytować",
                DELETE: "Usuń",
                EDIT_LOGO: 'Zmień logo',
                ADD_COVER_PHOTO: 'Dodaj cover (zdjęcie)',
                EDIT_COVER_PHOTO: 'Zmień cover (zdjęcie)',
                ADD_COVER_PHOTO_SHORT: 'Dodaj cover',
                EDIT_COVER_PHOTO_SHORT: 'Zmień cover',
                TITLE_EDIT_COLORS: 'Ustaw główne kolory dla projektu',
                SET_COLORS: 'Ustaw główne kolory',
                SET_COLORS_SHORT: 'Ustaw kolory',
                TITLE_1_EDIT_COLORS: 'Kolor podstawowy projektu',
                TITLE_1_DESC_EDIT_COLORS: 'Kliknij poniżej w pasek z kolorem aby wyświetlić paletę kolorów',
                TITLE_2_EDIT_COLORS: 'Kolor drugi (uzupełniający) projektu',
                TITLE_2_DESC_EDIT_COLORS: 'Kliknij poniżej w pasek z kolorem aby wyświetlić paletę kolorów',
                TITLE_3_EDIT_COLORS: 'Kolor tekstu na tle covera',
                TITLE_3_DESC_EDIT_COLORS: 'Kliknij poniżej w pasek z kolorem aby wyświetlić paletę kolorów',
                VIEW_DETAILS: "Pokaż szczegóły",
                EDIT_SOCIAL_MEDIA: "Edytuj linki do socjal mediów",
                ADD_SOCIAL_MEDIA: "Dodaj linki do socjal mediów"
            },
            MESSAGES: {
                INVALID_FORM: 'Formularz ma nieprawidłowe wartości. Proszę, napraw je, aby kontynuować',
                SHORT_DESC_UPDATED: 'Opis został pomyślnie zaktualizowany',
                TITLE_UPDATED: 'Tytuł projektu został pomyślnie zaktualizowany',
                TOKEN_UPDATED: 'Token został pomyślnie zaktualizowany',
                COLORS_UPDATED: 'Główne kolory zostały zaktualizowane',
                CONTENT_UPDATED: 'Opis został zaktualizowany',
                SOCIAL_MEDIA_UPDATED: 'Social Media projektu zostały zaktualizowane',
                EMPTY_DATA: 'Brakuje opisu projektu, uzupełnij aby kontynuować.'
            },
            FILES: {
                ADD_DOCUMENT: "Prześlij dokument",
                EDIT_DOCUMENT: "Edytuj dokument",
                CREATE_DOCUMENT: 'Utwórz dokument',
                SELECT_DOCUMENT_LANGUAGE: "Wybierz język dokumentu",
                LANGUAGE: {
                    ENGLISH: "język angielski",
                    POLISH: "Polskie"
                },
                DOWNLOAD: 'Pobierać',
                MESSAGES:{
                    DOCUMENT_SAVE: 'Dokument został pomyślnie utworzony',
                }
            },
            SETTINGS: {
                TITLE: 'Ustawienia projektu'
            },
            LINKS: {
                POS: 'https://docs.mosaico.ai/podstawowe-pojecia/inwestycja./czym-jest-pos',
                VESTING: 'https://docs.mosaico.ai/podstawowe-pojecia/inwestycja./czym-jest-vesting',
                MINT: 'https://docs.mosaico.ai/podstawowe-pojecia/mint-vs.-burn./czym-jest-mint',
                BURN: 'https://docs.mosaico.ai/podstawowe-pojecia/mint-vs.-burn./czym-jest-burn',
                INVESTMENT_PACKAGES: 'https://docs.mosaico.ai/podstawowe-pojecia/inwestycja./czym-sa-pakiety-inwestycyjne',
                DEFLATION: 'https://docs.mosaico.ai/podstawowe-pojecia/rynek./czym-jest-deflation',
                TOKEN_PRICE: 'https://docs.mosaico.ai/podstawowe-pojecia/rynek./czym-jest-cena-tokena',
                MIN_INVEST: 'https://docs.mosaico.ai/podstawowe-pojecia/inwestycja./czym-jest-min.-inwestycja',
                MAX_INVEST: 'https://docs.mosaico.ai/podstawowe-pojecia/inwestycja./czym-jest-maks.-inwestycja',
                SOFT_CAP: 'https://docs.mosaico.ai/podstawowe-pojecia/soft-cap-vs.-hard-cap/czym-jest-soft-cap',
                HARD_CAP: 'https://docs.mosaico.ai/podstawowe-pojecia/soft-cap-vs.-hard-cap/czym-jest-hard-cap',
                TOTAL_SUPPLY: 'https://docs.mosaico.ai/podstawowe-pojecia/rynek./czym-jest-calkowita-podaz',
                STAGES: 'https://docs.mosaico.ai/podstawowe-pojecia/rynek./czym-sa-etapy-projektu',
                TOKENOMICS: 'https://docs.mosaico.ai/podstawowe-pojecia/rynek./czym-jest-tokenomica'
            }
        },
        PROJECT_MEMBERS: {
            CANNOT_REMOVE_YOURSELF: "Nie możesz usunąć siebie",
            CANNOT_REMOVE_PROJECT_CREATOR: "Nie możesz usunąć twórcy projektu"
        },
        SOCIAL_LINKS: {
            CARD: {
                TITLE: 'Media społecznościowe Projektu',
                TELEGRAM: 'Kanał telegramu',
                YOUTUBE: 'Kanał Youtube',
                LINKEDIN: 'Profil LinkedIn',
                FACEBOOK: 'Strona na Facebooku',
                TWITTER: 'Profil na Twitterze',
                INSTAGRAM: 'Profil na Instagramie',
                MEDIUM: 'Profil na Medium',
                TIKTOK: 'Profil na TikToku'
            },
            MESSAGE: {
                SUCCESS: 'Zaktualizowano polubienia w mediach społecznościowych.',
                FAILED: 'Dane nie zostały zapisane! Sprawdź i spróbuj ponownie.',
                INVALID_URL: 'Wpisałeś nieprawidłowy adres URL (adres URL musi zaczynać się od https)'
            }
        },
        PROJECT_FOOTER: {
            COMPANY: {
                MORE_DETAILS: 'Sprawdź więcej'
            }
        },
        BUY: {
            back_to_project: 'Wróć do projektu'
        },
        EDITOR: {
            ACTIONS: {
                SAVE: "Zapisz",
                RESET: "Resetowanie",
                CANCEL: "Anuluj"
            },
            RESET: {
                TITLE: "Potrzebujesz potwierdzenia",
                MESSAGE: "Czy na pewno chcesz zrezygnować? Wszystkie twoje postępy zostaną utracone.",
                ACTIONS: {
                    CONFIRM: "tak",
                    CANCEL: "Nie"
                }
            },
            MESSAGES: {
                SUCCESS: 'Dokument został pomyślnie zapisany',
                INVALID_VALUE: 'Nie można zapisać pustej treści'
            }
        },
        PROJECT_SETTINGS: {
            TRANSACTIONS: {
                TITLE: 'Transakcje',
                TABLE: {
                    COLUMNS: {
                        USER_NAME: 'Nazwa użytkownika',
                        PROJECT_NAME: 'Nazwa projektu',
                        TRAN_ID: 'Identyfikator transakcji',
                        PACKAGE_NAME: 'Nazwa pakietu',
                        PAID: 'Zapłacono',
                        PURCHASE_DATE: 'Data zakupu',
                        BALANCE: 'Saldo $',
                        BALANCE_GOODS: 'Saldo towarów',
                        SOURCE: 'Źródło'
                    },
                },
                NO_TRANSACTIONS: 'Brak transakcji',
                MESSAGES: {
                    NO_TRANSACTIONS_TO_BE_EXPORTED: 'Brak transakcji możliwych do eksportu'      
                }
            },
            INVESTORS: {
                TITLE: 'Investors',
                COPY_HINT: "Skopiuj",
                TABLE: {
                    COLUMNS: {
                        USER_NAME: 'Nazwa użytkownika',
                        PHONE_NUMBER: 'Numer telefonu',
                        WALLET_ADDRESS: 'Adres portfela',
                        USDT_BALANCE: 'Saldo USDT',
                        USDC_BALANCE: 'Saldo USDC',
                        MATIC_BALANCE: 'Saldo MATIC',
                        BANK_TRANSFER: 'Przelew bankowy',
                        TOTAL_INVESTMENT: 'Cala inwestycja'
                    }
                },
                NO_INVESTORS: 'Brak inwestorów do wyświetlenia.'
            }
        },
        MODALS: {
            NEW_TOKEN: {
                TITLE: 'Utwórz token'
            },
            CAMPAIGN_EDITOR: {
                TITLE: 'Kampania',
                FORM: {
                    NAME: {
                        LABEL: "Pseudonim artystyczny",
                        ERROR: "Błędna nazwa",
                        PLACEHOLDER: "Wpisz nazwę sceniczną"
                    },
                    SUPPLY: {
                        LABEL: "Ilość tokenów",
                        PLACEHOLDER: 'Podaj ilość tokenów przeznaczonych na tę rundę',
                        ERROR: 'niewłaściwa wartość'
                    },
                    TOKEN_PRICE: {
                        LABEL: "Cena tokena",
                        PLACEHOLDER: 'Wpisz cenę tokena dla tej rundy',
                        ERROR: 'niewłaściwa wartość'
                    },
                    MIN_PURCHASE: {
                        LABEL: "Minimalny zakup",
                        PLACEHOLDER: 'Podaj minimalną ilość tokenów do zakupu',
                        ERROR: 'niewłaściwa wartość'
                    },
                    MAX_PURCHASE: {
                        LABEL: "Maksymalny zakup",
                        PLACEHOLDER: 'Podaj maksymalną ilość tokenów do zakupu',
                        ERROR: 'niewłaściwa wartość'
                    },
                    START_DATE: {
                        LABEL: "Data rozpoczęcia",
                        PLACEHOLDER: 'Wybierz datę',
                        ERROR: 'niewłaściwa wartość'
                    },
                    END_DATE: {
                        LABEL: "Data zakonczenia",
                        PLACEHOLDER: 'Wybierz datę',
                        ERROR: 'niewłaściwa wartość'
                    },
                    PRIVATE_SALE: {
                        LABEL: 'Prywatna sprzedaż'
                    },
                    ACTIONS: {
                        SAVE: 'Zapisz',
                        ADD: 'Dodaj etap',
                        DELETE: 'Usuń'
                    }
                },
                MESSAGES: {
                    UPDATE_SUCCESS: "Kampania została pomyślnie zaktualizowana",
                    INVALID_FORM: "Form contains niewłaściwa wartośćs"
                }
            },
            PROJECT_LOGO_EDITOR: {
                TITLE: "Edytuj logo projektu",
                ACTIONS: {
                    SAVE: "Zapisz",
                    CANCEL: 'Anuluj',
                    ADD: 'Dodaj logo',
                    CHANGE: 'Zmień logo'
                },
                MESSAGES: {
                    SUCCESS: "Logo zostało pomyślnie zaktualizowane",
                    INVALID_FORM: "Dane są nieprawidłowe. Nie można zapisać"
                }
            },
            COVER_UPLOAD: {
                TITLE: "Obraz okładki projektu",
                ACTIONS: {
                    ADD: "Dodać",
                    CHANGE: "Zmiana"
                },
                MESSAGES: {
                    SUCCESS: "Cover został pomyślnie zaktualizowany",
                    INVALID_FORM: "Dane są nieprawidłowe. Nie można zapisać"
                }
            },
            PROJECT_ARTICLE_PHOTO_UPLOAD: {
                TITLE: "Edytuj zdjęcie autora",
                ACTIONS: {
                    SAVE: "Zapisz",
                    CANCEL: 'Anuluj',
                    ADD: 'Dodaj zdjęcie autora',
                    CHANGE: 'Edytuj zdjęcie autora'
                },
                MESSAGES: {
                    SUCCESS: "Zdjęcie autora zostało pomyślnie zaktualizowane",
                    INVALID_FORM: "Dane są nieprawidłowe. Nie można zapisać"
                }
            },
            PROJECT_ARTICLE_COVER_UPLOAD: {
                TITLE: "Edytuj zdjęcie na okładkę",
                ACTIONS: {
                    SAVE: "Zapisz",
                    CANCEL: 'Anuluj',
                    ADD: 'Dodaj zdjęcie (cover)',
                    CHANGE: 'Edytuj zdjęcie (cover)'
                },
                MESSAGES: {
                    SUCCESS: "Zdjęcie (cover) zostało pomyślnie zaktualizowane",
                    INVALID_FORM: "Dane są nieprawidłowe. Nie można zapisać"
                }
            },
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
            EDIT_BANK_DATA: {
                TITLE: "Zmień dane do przelewu",
                SUBTITLES: {
                    ACCOUNT_NUMBER: "Numer Konta",
                    BANK_NAME: "Nazwa banku",
                    SWIFT: "Swift",
                    ACCOUNT_ADDRESS: "Adres konta"
                },
                PLACEHOLDERS: {
                    ENTER_ACCOUNT: "Wprowadź numer konta",
                    ENTER_BANK_NAME: "Aprowadź nazwę banku",
                    ENTER_SWIFT: "Wprowadź swift",
                    ENTER_ACCOUNT_ADDRESS: "Wprowadź adres konta"
                },
                MESSAGES: {
                    SUCCESS: "Pomyślnie zaktualizowano dane do przelewu"
                },
                ACTIONS: {
                    UPDATE: "Aktualizuj",
                    CANCEL: "Anuluj"
                },
                FORM:{
                    ACCOUNT: {
                        ERROR: "Pole wymagane",
                    },
                    BANK_NAME: {
                        ERROR: "Pole wymagane"
                    },
                    SWIFT: {
                        ERROR: "Pole wymagane"
                    },
                    ACCOUNT_ADDRESS: {
                        ERROR: "Pole wymagane"
                    },
                    MESSAGES: {
                        ERROR: "Formularz zawiera błędne dane. Wprowadź poprawne wartości aby kontunuować"
                    },
                }
            },
            STAKING_TOKEN_CLAIM: {
                MESSAGES: {
                    CLAIM_TOKEN_COST: "Odbierasz tokeny przed zakończeniem umowy, wiąże się to z dodatkową opłatą oraz utratą nagrody.",
                    ARE_YOU_SURE: "Na pewno chcesz wypłacić środki?",
                    FEE_COST: "Koszt dodatkowej opłaty: ",
                    DO_YOU_WANT: "Czy chcesz wymienić wszystkie WMOS na MOS?",
                    YOU_WILL_RECIEVE: "Otrzymasz ... MOS.",
                    SUCCESSFULY_EXCHANGED: "Wmieniłeś WMOS na MOS.",
                    PLEASE_NOTE: "środki pojawią się na Twoim portfelu w ciągu 48h."
                },
                ACTIONS: {
                    CLAIM: "Wypłać",
                    CANCEL: "Anuluj",
                }
            }
        },
        PROJECT_SCORE: {
            TITLE: "Uzupełnij informacje o projekcie, aby rozpocząć kampanię!",
            DESCRIPTION: "Uzupełnienie wszystkich niezbędnych informacji sprawi, że Twój projekt będzie bardziej atrakcyjny dla zróżnicowanych grup ludzi.",
            ACTIONS: {
                EXPAND_ERRORS: "Czego brakuje?",
                SUBMIT: "Składać"
            }
        },
        SUBSCRIPTION_TO_NEWSLETTER: {
            TITLE: "Zapisz się na alert inwestora",
            ACTIONS: {
                SAVE: "Zapisz się",
                CANCEL: 'Anuluj',
                SIGN_ME_OUT: 'Wypisz mnie',
                // ADD: 'Add logo',
                // CHANGE: 'Edit logo'
            },
            MESSAGES: {
                INFO: "Chcesz dowiedzieć się więcej na temat projektu? Być jednym z pierwszych inwestorów? Zapisz się na naszą whitelistę!",
                INFO_FOR_NOT_LOGGED_IN: "Narazie tylko dla zalogowanych!",
                INFO_FOR_NOT_LOGGED_IN2: "Zaloguj się",
                INFO_ALREADY_SUBSCRIBED: "Jesteś juz zapisany do newslettera projektu.",
                INFO_SUBSCRIBED_TO_NEWSLETTER_LOGGED_IN: "Zapisano do newslettera!",
                INFO_SUBSCRIBED_TO_NEWSLETTER_NOT_LOGGED_IN: "Przejdź do swojej skrzynki pocztowej i kliknij w znajdujący się w niej link, aby potwierdzić zapisanie się do newslettera.",
                INFO_UNSUBSCRIBED_TO_NEWSLETTER: "Wypisano z newslettera!",
                INFO_CONFIRM_UNSUBSCRIBED: "Czy chcesz się wypisać z newslettera?",
            }
        },
        PRIVATE_SALE_SUB: {
            ENABLED: "Gratulacje! Możesz teraz kupować tokeny ",
            ACTIONS: {
                PURCHASE: "Kup tokeny"
            }
        },
        VALIDATION_NO_DESCRIPTION: "Dodaj opis projektu w dziale „O programie”",
        VALIDATION_NOT_FILLED_COMPANY: "Dodaj więcej informacji o swojej firmie. Dokończ weryfikację firmy.",
        VALIDATION_NO_PAGE_COVER: "Zmień zdjęcie na okładkę",
        VALIDATION_NO_STAGES: "Dodaj kampanię sprzedażową w sekcji „Ustawienia”",
        VALIDATION_NO_TOKEN: "Wybierz token, który chcesz sprzedać",
        VALIDATION_NO_TITLE: "Dodaj tytuł projektu",
        VALIDATION_NO_SHORT_DESCRIPTION: "Nie ma krótkiego opisu. Pomóż inwestorom szybciej znaleźć Twój projekt",
        VALIDATION_NO_FAQ: "Nie masz żadnego elementu FAQ w projekcie. Dodaj kilka z nich, aby odpowiedzieć na najczęściej zadawane pytania dotyczące Twojego projektu",
        INVALID_PROJECT: "Brakuje informacji. Sprawdź wynik projektu i napraw sugerowane problemy, aby kontynuować",
        PROJECT_JOINED: "Dołączyłeś do projektu",
        VALIDATION_NO_DOCUMENTS: "Upewnij się, że dostarczyłeś co najmniej 3 dokumenty: Whitepaper, Regulamin i Polityka prywatności",
        VALIDATION_NO_TEAM: "Społeczność zazwyczaj wymaga od członków zespołu przejrzystości i znajomości. Dodaj kilku członków zespołu w sekcji „O mnie”",
        VALIDATION_NO_SOCIAL_MEDIA: 'Upewnij się, że odpowiednio dbasz o swoje media społecznościowe. Dodaj odniesienia do Facebooka, Telegramu itp..',
        PACKAGES_Add_new_package: "Dodaj nowy pakiet",
        PACKAGES_Edit_list: "Lista pakietów",
        PACKAGES_Edit_package: "Edytuj pakiet",
        PACKAGE_ADDED: "Szczegóły pakietu zostały pomyślnie dodane",
        PACKAGE_DELETED: "Usunięto pakiet inwestorski",
        MODAL_PACKAGE_title: "Pakiety inwestycyjne",
        INFO_UNDER_IMAGE: 'Zmień zdjęcie domyślne (opcjonalnie)',
        PACKAGE_addPhotoText: 'Dodaj zdjęcie picture',
        PACKAGE_changePhotoText: 'Zmień zdjęcie',
        PACKAGE_package_name: 'Nazwa pakietu',
        PACKAGE_placeholder_package_name: 'Wprowadź nazwę pakietu',
        PACKAGE_incorect_package_name: 'Niepoprawna nazwa pakietu',
        PACKAGE_invalid_form: 'Formularz zawiera niepoprawne wartości. Aby kontynuować uzupełnij pola.',
        PACKAGE_tokens_amount: 'Liczba tokenów',
        PACKAGE_placeholder_tokens_amount: 'Wprowadź liczbę tokenów',
        PACKAGE_incorect_tokens_amount: 'Niepoprawna wartość',
        PACKAGE_benefits_title: 'Dodatki',
        PACKAGE_benefits_title_info: 'Możesz dodać do 5 dodatków',
        PACKAGE_benefit_1: 'Dodatek 1',
        PACKAGE_benefit_2: 'Dodatek 2 (opcjonalnie) ',
        PACKAGE_benefit_3: 'Dodatek 3 (opcjonalnie) ',
        PACKAGE_benefit_4: 'Dodatek 4 (opcjonalnie) ',
        PACKAGE_benefit_5: 'Dodatek 5 (opcjonalnie) ',
        PACKAGE_placeholder_benefit_1: '',
        PACKAGE_placeholder_benefit_2: '',
        PACKAGE_placeholder_benefit_3: '',
        PACKAGE_placeholder_benefit_4: '',
        PACKAGE_placeholder_benefit_5: '',
        PACKAGE_incorect_benefit: 'Niepoprawna wartość',
        PACKAGE_no_list: 'Nie ma dodanych pakietów inwestycyjnych',
        PRESS_ROOM_invalid_form: 'Formularz zawiera błędne dane. Wprowadź poprawne wartości aby kontunuować.',
        TOKEN_NOT_DEPLOYED: 'Upewnij się, że token został utworzony na blockchain',
        MODAL_FAQ_title: "",
        FAQ_Add_new_faq: "Dodaj faq",
        FAQ_Edit_list: "Lista faq",
        FAQ_Edit_faq: "Edytuj faq",
        FAQ_DELETED: "FAQ został usunięty",
        FAQ_question_name: 'Pytanie',
        FAQ_placeholder_question_name: 'Dodaj tutaj popularne pytanie',
        FAQ_incorect_question_name: 'Nieprawna wartość',
        FAQ_answer_name: 'Odpowiedź',
        FAQ_placeholder_answer_name: 'Wprowadź odpowiedź',
        FAQ_incorect_answer_name: 'Nieprawna wartość',
        FAQ_is_Faq_hidden: 'Ukryty faq?',
        FAQ_no_list: 'Nie ma dodanego FAQ',
        FAQ_delete_title: 'Are you sure you want to delete an FAQ?',
        FAQ_delete_description: 'It will be removed for forever ',
        INSUFFICIENT_FUNDS: "Nie masz wystarczających środków",
        GAS_TOO_EXPENSIVE: "Gaz jest za drogi",
        PROJECT_PURCHASE: {
            RAMP: "Zapłać przez Ramp",
            INIT_BANK_TRANSFER: "Szczegóły płatności",
            TRANSAK: "Zapłać przez Transak",
            CONFIRM_BANK_TRANSFER: "Potwierdzam",
            METAMASK: "Zapłać",
            COPY_SUCCESS: 'Skopiowane',
            COPY_HINT: "Skopiuj",
            TRANSACTION_INIT: "Transakcja rozpoczęta. Poczekaj na potwierdzenie.",
            BANK_TRANSFER: {
                REMARK: "* Czas dokonania transakcji wynosi do 8 godzin roboczych od momentu wpłynięcia środków na nasz rachunek bankowy.",
                SWIFT: "SWIFT/BIC",
                DETAILS: "Szczegóły płatności",
                ACCOUNT: "Na konto",
                REFERENCE: "Tytuł",
                RECIPIENT: "Odbiorca",
                BANK: "Nazwa banku",
                KYC_ALERT: {
                    TITLE: "KYC jest wymagany",
                    CONTENT: "KYC jest wymaganą procedurą weryfikacji tożsamości w celu zapobiegania oszustwom, korupcji, praniu pieniędzy i finansowaniu terroryzmu. Musisz zakończyć weryfikację przed kontynuacją.",
                    COMPLETE: "Zweryfikuj tożsamość"
                }
            }
        },
        CHECKOUT: {
            TOKENS: "tokenów",
            MIN_TOKENS: "Możesz kupić min",
            MAX_TOKENS: "Możesz kupić max.",
            REQUIRED_FIELD: 'Pole wymagane'
        },
        CREDIT_CARD_PAYMENT: {
            EXPLAINER: {
                TITLE: "Informacje o opłatach",
                ACTIONS: {
                    ACCEPT: "Akceptuj",
                    ACCEPT_NO_SHOW: "Akceptuj i nie pokazuj tego okna w przyszłości."
                },
                CONTENT: "<p>Ten<strong> procesor płatniczy wymieni wybraną przez Ciebie walutę na stablecoin</strong>, aby móc zakupić tokeny projektu. </p> <p>Proszę pamiętać, że z powodu <strong>dynamicznie zmieniających się wartości walut, liczba tokenów może się nieznacznie różnić.</strong> </p> <p>Jeśli chcesz być po bezpiecznej stronie, zamów kilka tokenów więcej.</p> <p>Możesz zignorować wyświetlane wartości walut i prezentowane opłaty w procesorze płatności.</p> <p>Obliczone opłaty to tylko informacja podana automatycznie przez zewnętrznego dostawcę.</p> <p>Nie płacisz żadnych dodatkowych opłat. Wszystkie wyświetlane opłaty są pokrywane przez twórcę projektu.</p>"
            }
        },
        "ORDER_CONFIRMATION": {
            "CONTENT": "Dziękujemy za zakupy. Proszę czekać na informację o statusie transakcji.",
            "ACTIONS": {
              "INVEST": "Zainwestuj ponownie",
              "CHECK_TX": "Sprawdź transakcję"
            }
        },
        PRESS_ROOM: {
            TITLE: "Press Room",
            ACTIONS: {
                ADD_NEW: "Dodaj nowe",
                READ_MORE: "Więcej"
            },
            TITLE_ADD: 'Dodaj nowy artykuł',
            TITLE_EDIT: 'Edytuj opis artykułu',
            DELETE_TITLE: 'Are you sure you want to delete an article?',
            DELETE_DESCRIPTION: 'It will be removed for forever',
            ADD_COVER: 'Dodaj zdjęcie',
            ARTICLE_LINK: 'Link do artykułu',
            INCORRECT_ARTICLE_LINK: 'Nieprawidłowy link do artykułu',
            TEXT_VISIBLE: 'Opis artykułu',
            IMG_VISIBLE: 'Displayed image',
            INCORRECT_TEXT_VISIBLE: 'Nieprawidłowy tekst',
            ADDITIONAL_INFO: 'Dodatkowe informacje',
            ADD_AUTHOR_PHOTO: 'Dodaj zdjęcie autora (opcjonalnie)',
            CHANGE_AUTHOR_PHOTO: 'Zmień zdjęcie autora',

            ADD_ARTICLE_AUTHOR: 'Dodaj nazwisko autora artykułu (opcjonalnie)',
            INCORRECT_ARTICLE_AUTHOR: 'Nieprawidłowe dane autora artykułu',

            ADD_ARTICLE_DATE: 'Dodaj datę artykułu (opcjonalnie)',
            INCORRECT_ARTICLE_DATE: 'Nieprawidłowa data',
            ARTICLE_NO_LIST: 'Jeszcze nie dodano artykułów',
            ARTICLE_HIDDEN: 'Ukryty'
        },
        ARTICLE:{
            MESSAGES:{
                HIDE_SUCCESS:'Artykuł został pomyślnie ukryty',
                DISPLAY_SUCCESS: 'Artykuł został wyświetlony pomyślnie',
                CREATE_SUCCESS: 'Dodano artykuł',
                UPDATE_SUCCESS: 'Artukuł został pomyślnie zaktualizowany'
            }
        },
    }
};

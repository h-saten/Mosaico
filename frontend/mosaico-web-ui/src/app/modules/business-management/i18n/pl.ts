export const locale = {
    lang: 'pl',
    data: {
        PUBLIC_COMPANIES: {
            HEADER_TITLE: "Firmy, które <br>nam <span class='title-mosaico'>zaufały</span>",
            HEADER_DESC: "Zdecentralizowana organizacja autonomiczna, reprezentowana przez reguły zakodowane jako program komputerowy, który jest przejrzysty, kontrolowany przez członków organizacji.",
            TRUESTED_COMPANIES: "Zaufane firmy",
            SEARCH: "Szukaj",
            COMPANY_DESC: "Opis Firmy",
            VIEW_DAO: "Wyświetl stronę DAO",
            FOLLOW: "Śledź DAO",
            UNFOLLOW: "Przestań obserwować",
            VIEW_MORE: "Zobacz więcej stron DAO",
            LOADING: "Ładowanie...",
            BTN: {
                BUILD_DAO: "Zbuduj swoje DAO"
            },
            NO_COMPANIES: "Brak wyników",
            OPEN_POLLS: "Otwarte",
            TOTAL_POLLS: "Łącznie"
        },
        COMPANY_OVERVIEW: {
            TOKEN_LIST_TITLE: "Lista tokenów",
            NO_TOKENS: 'Brak tokenów',
            NO_DOCUMENTS: 'Brak dokumentów',
            NO_PROJECTS: 'Brak projektów',
            ACTIONS: {
                CREATE: 'Utwórz token',
                IMPORT: 'Importuj token',
                SUBSCRIBE: 'Subskrybuj',
                UNSUBSCRIBE:"Wypisz się"
            }
        },
        COMAPNY_DOCUMENT:{
            TITLE:"Dokumenty",
            LANGUAGE:{
                TITLE:"Wybierz język"
            },
            ACTIONS:{
                ADD_FILES: "Dodaj dokument"
            }
        },
        COMPANY_DESCRIPTION: {
            TITLE: 'Opis Firmy'
        },
        COMPANY_PROJECTS: {
            TITLE: 'Nasz projekt',
            ACTIONS: {
                CREATE: 'Utwórz projekt',
                DETAILS: 'Sprawdź projekt'
            }
        },
        COMPANY_PAGE_MENU: {
            OVERVIEW: "Przegląd",
            VOTING: "Głosowanie",
            HOLDERS: "Hodlers",
            MEMBERS: "Członkowie",
            WALLET: "Portfel",
            SETTINGS: "Ustawienia",
            SOON: 'Już wkrótce'
        },
        COMPANY_INFO_CARD: {
            CONTACTS_TITLE: "Łączność",
            OFFICE_TITLE: "Nasze biuro",
            SOCIAL_TITLE: "Znajdź nas online",
            TAX_ID: "NIP"
        },
        COMPANY_SETTINGS_TABS: {
            DETAILS: 'Detale',
            SOCIAL: 'Media społecznościowe',
            MEMBERS: 'DAO Członkowie',
            KYB: 'Weryfikacja'
        },
        SOCIAL_LINKS: {
            CARD: {
                TITLE: 'Media społecznościowe dla Twojego DAO',
                TELEGRAM: 'Kanał telegramu',
                YOUTUBE: 'Kanał Youtube',
                LINKEDIN: 'Profil LinkedIn',
                FACEBOOK: 'Strona na Facebooku',
                TWITTER: 'Profil na Twitterze',
                INSTAGRAM: 'Profil na Instagramie',
                MEDIUM: 'Średni'
            },
            MESSAGE: {
                SUCCESS: 'Zaktualizowano polubienia w mediach społecznościowych.',
                FAILED: 'Dane nie zostały zapisane! Sprawdź i spróbuj ponownie.',
                INVALID_URL: 'Wpisałeś nieprawidłowy adres URL (adres URL musi zaczynać się od https)'
            }
        },
        MEMBERS: {
            INVITATIONS: 'Zaproszenia',
            STATUS: 'Status',
            ROLE: 'Rola',
            EXPIRATION: 'Wygaśnięcie',
            ACTIONS: 'Operacje'
        },
        COMPANY_VOTING: {
            TITLE: 'Głosowanie',
            ACTIONS: {
                CREATE: "Dodaj propozycję"
            },
            NO_PROPOSALS: "Obecnie brak propozycji",
            CREATED_BY: "Stworzył",
            FOR: "Tak",
            AGAINST: "Nie",
            MESSAGES: {
                COPIED: "Skopiowane",
                VOTE_SUCCESS: 'Głos został zapisany'
            },
            TIME_LEFT: "Zamyka się o",
            STARTS_AT: 'Zaczyna się o',
            CLOSED: 'Zamknięte',
            TOTAL_VOTED: 'Zagłosowali'
        },
        COMPANY_EDIT: {
            CONTACT_INFO_TITLE: 'Informacje kontaktowe',
            FORM: {
                NAME: {
                    LABEL: 'DAO Nazwa',
                    PLACEHOLDER: 'Wchodzić DAO Nazwa',
                    ERROR: 'imie jest wymagane'
                },
                DESCRIPTION: {
                    LABEL: 'Opis Firmy',
                    PLACEHOLDER: 'Wprowadź opis firmy',
                },
                COUNTRY: {
                    LABEL: 'Kraj',
                    PLACEHOLDER: 'Wybierz kraj'
                },
                NETWORK: {
                    LABEL: 'Sieć Blockchain',
                    ERROR: 'Sieć jest wymagana',
                    PLACEHOLDER: "Wybierz swój łańcuch bloków"
                },
                STREET: {
                    LABEL: 'Ulica',
                    PLACEHOLDER: 'Wpisz adres swojej rejestracji'
                },
                POSTAL_CODE: {
                    LABEL: 'Kod pocztowy',
                    PLACEHOLDER: 'Wpisz kod pocztowy'
                },
                REGION: {
                    LABEL: "Państwo / Województwo",
                    PLACEHOLDER: "Wpisz region/prowincję/stan"
                },
                SIZE: {
                    LABEL: 'Wielkość organizacji',
                    PLACEHOLDER: 'Wybierz wielkość swojej firmy',
                    OPTIONS: {
                        SMALL: 'Mały (1-49)',
                        MEDIUM: 'Średni (50-249)',
                        LARGE: 'Wielki (250+)'
                    }
                },
                VAT: {
                    LABEL: "NIP",
                    PLACEHOLDER: "Wpisz identyfikator firmy"
                },
                PHONE: {
                    LABEL: "Numer telefonu",
                    PLACEHOLDER: "Wpisz numer telefonu"
                },
                EMAIL: {
                    LABEL: "E-mail",
                    PLACEHOLDER: "Wprowadź adres e-mail"
                },
                LEGAL_SECTION: 'Prawny',
                SETTINGS_SECTION: 'Ustawienia',
                VOTING: {
                    LABEL: 'Włącz głosowanie'
                },
                EVERYBODY_CREATES: {
                    LABEL: 'Każdy może oddać głos'
                }
            },
            TITLE: "Tworzyć DAO",
            MESSAGES: {
                INVALID_FORM: 'Wprowadzone wartości są nieprawidłowe. Proszę spróbuj ponownie',
                SUCCESS: 'DAO zostało pomyślnie zapisane',
                LEFT: 'Pomyślnie opuścił DAO'
            },
            LEAVE_COMPANY_MODAL: {
                TITLE: 'Potwierdź swoją decyzję',
                MESSAGE: 'Czy na pewno chcesz opuścić to DAO??'
            },
            ACTIONS: {
                LEAVE: 'Opuść DAO',
                SAVE: 'Zapisz',
                SAVING: 'Oszczędność...',
                CANCEL: 'Anuluj',
                UPDATE: 'Zapisz'
            }
        },
        COMPANIES: {
            MY: {
                TITLE: "DAO",
                NO_COMPANIES: "Jeszcze nie jesteś częścią żadnego DAO",
                DESCRIPTION: "Możesz stworzyć własne DAO czyli zdecentralizowaną firmę. <b> Nasza definicja DAO </b> DAO (zamiennie DAC) - Nowy format organizacji (np. firmy), która dzięki decentralizacji, jest transparentna (publiczna) i zdemokratyzowana, a dzięki tokenizacji, umożliwia zbiórkę kapitału na jej rozwój, obrót na rynku wtórnym oraz zarządzanie spółką poprzez głosowanie i relacje inwestorskie. DAO staje się podmiotem odpowiedzialnym za zbiórkę, jej zarząd może zostać odwołany, a zebrane środki są dostępne do wglądu dla Tokenariuszy.",
                ACTIONS: {
                    DETAILS: 'Pogląd',
                    WALLET: 'Portfel',
                    CREATE: 'Utwórz DAO'
                }
            }
        },
        COMPANY_WALLET: {
            SUMMARY: {
                TOTAL: 'Aktywa ogółem',
                COPY_HINT: 'Skopiuj do schowka',
                ACTIONS: {
                    DEPOSIT: 'Wpłata',
                    SEND: 'Wyślij',
                    WITHDRAW: 'Wypłać'
                },
                MESSAGES: {
                    COPIED: 'Skopiowane!'
                }
            },
            STATS: {
                COMING_SOON: 'Niedługo uruchomimy statystyki',
                GET_NOTIF: "Otrzymywać powiadomienia"
            },
            ASSETS: {
                TITLE: "Aktywa",
                NO_ASSETS: "Nie ma aktywów. Czas to zmienić...",
                ACTIONS: {
                    EXCHANGE: 'Giełda',
                    STAKE: 'Stawka',
                    MANAGE: 'Zarządzanie'
                }
            },
            TRANSACTIONS: {
                TITLE: "Transakcje",
                TABLE: {
                    TOKEN: 'Znak',
                    HASH: 'Transakcja',
                    SOURCE: 'Źródło',
                    AMOUNT: 'Kwota',
                    DESTINATION: 'Miejsce docelowe'
                },
                NO_TRANSACTIONS: 'Brak transakcji',
                ACTIONS: {
                    LOAD_MORE: 'Zobacz więcej'
                }
            }
        },
        MODALS: {
            NEW_TOKEN: {
                TITLE: 'Utwórz token'
            },
            IMPORT_TOKEN: {
                TITLE: 'Importuj token'
            },
            COMPANY_LOGO_EDITOR: {
                TITLE: "Edytuj logo firmy",
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
            WALLET_SEND: {
                TITLE: 'Wyślij środki',
                ACTIONS: {
                    SEND: 'Wysłać',
                    CANCEL: 'Anuluj'
                },
                FORM: {
                    TOKEN: {
                        LABEL: 'Znak',
                        PLACEHOLDER: "np USDT",
                        INVALID: 'Nieprawidłowy Token'
                    },
                    ADDRESS: {
                        LABEL: 'Adres przeznaczenia',
                        PLACEHOLDER: 'np 0x71C7656EC7ab88b098defB751B7401B5f6d8976F',
                        INVALID: 'Błędny adres'
                    },
                    AMOUNT: {
                        LABEL: 'Kwota',
                        PLACEHOLDER: 'np 25.00',
                        INVALID: 'nieprawidłowa kwota'
                    },
                    CURRENT_BALANCE: 'Aktualne saldo'
                },
                MESSAGES: {
                    INVALID_FORM: 'Formularz zawiera nieprawidłowe wartości',
                    SUCCESS: 'Twoja transakcja zostanie wkrótce przetworzona'
                }
            },
            NEW_COMPANY: {
                TITLE: 'Nowe DAO'
            },
            NEW_PROPOSAL: {
                MESSAGES: {
                    SUCCESS: "Pomyślnie utworzona",
                    INVALID_FORM: "Formularz zawiera nieprawidłowe wartości",
                    TRANSACTION_INITIATED: "Transakcja została zainicjowana. Proszę czekać"
                },
                TITLE: "Nowa propozycja",
                ACTIONS: {
                    CANCEL: "Anuluj",
                    CREATE: "Zapisz"
                },
                FORM: {
                    TITLE: {
                        LABEL: "Tytuł",
                        ERROR: "Błąd tytułu",
                        PLACEHOLDER: "Wpisz krótkie pytanie"
                    },
                    DESCRIPTION: {
                        LABEL: "Opis",
                        ERROR: "Błąd opisu",
                        PLACEHOLDER: "Powiedz więcej"
                    },
                    QUORUM: {
                        LABEL: "Próg kworum",
                        ERROR: "Invalid threshold",
                        PLACEHOLDER: "Wpisz, ile % posiadaczy tokenów powinno głosować"
                    },
                    TOKEN: {
                        LABEL: "Token",
                        ERROR: "Invalid token",
                        PLACEHOLDER: "Wybierz token, którego dotyczy propozycja"
                    }
                }
            }
        },
        HOLDERS_TAB: {
            NO_HODLERS: "Brak hodlerów",
            BALANCE: "Bilans",
            WALLET_ADDRESS: "Adres portfela",
            FILTER_BY: "Filtruj według:"
        },
        INSUFFICIENT_FUNDS: "Brak środków"
    }
};

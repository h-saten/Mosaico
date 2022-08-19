export const locale = {
    lang: 'pl',
    data: {
        MANUAL_DEPOSIT: {
            TITLE: "Przelew ręczny",
            ACTIONS: {
                DONE: "OK",
                COPY_HINT: "Skopiuj adres"
            },
            ADDRESS_COPIED: "Adres skopiowany",
            INFO: "Prosimy o wykonanie ręcznego transferu pożądanych tokenów (Matic, <a href='https://polygonscan.com/token/0xc2132d05d31c914a87c6611c10748aeb04b58e8f' target='_blank'>USDT</a> lub <a href='https://polygonscan.com/token/0x2791bca1f2de4661ed88a30c99a7a9449aa84174' target='_blank'>USDC</a>) na poniższy adres. " +
                "<span class='fw-bold'>Przelew powinien zostać wykonany za pośrednictwem sieci Polygon.</span> Twoje saldo zostanie automatycznie zaktualizowane w ciągu 5 minut od potwierdzenia transakcji."
        },
        WALLET_OVERVIEW: {
            DASHBOARD: "Dashboard",
            STAKING: "Staking",
            VESTING: "Vesting",
            LEARN_MORE: "Więcej",
            AFFILIATION: "Afiliacja"
        },
        WALLET_STAKING: {
            FORM: {
                ACCEPT_WITH_TERMS: "Przeczytałem i akceptuję <a href='{{termsAndConditionsUrl}}' target='_blank'>Regulamin</a>",
                ACCEPT_WITHOUT_TERMS: 'Rozumiem i akceptuję ewentualne ryzyko związane ze stakingiem'
            },
            DISCLAIMER: {
                READ_MORE: "czytaj dalej",
                STANDARD: {
                    SHORT: "Ten staking zamraża Twoje tokeny na nieokreślony okres czasu...",
                    FULL_INFO: "This staking freezes your tokens for an undefined period of time. You can withdraw tokens any time, but you will not be eligable for next reward release. Adding tokens to this staking is possible and it will influence the amount of reward you will be eligible for, in accordance with the principles described in the whitepaper. Estimated APR is calculated based on Project’s whitepaper and declared periodical yield of the issuer. The declared yield will be verified shortly before the next reward drop, based on actual values provided by the issuer.",
                    TITLE: "Staking information"
                }
            },
            STATISTICS: {
                TITLE: {
                    TOTAL_IN_STAKING: "Suma Staking",
                    ACTIVE_STAKING: "Aktywny Staking",
                    REWARD_CLAIMED: "Odebrana nagroda"
                }
            },
            ASSETS: {
                AMOUNT: {
                    PLACEHOLDER: 'Ilość'
                },
                TITLE: {
                    STAKED_ASSETS: "Wybierz aktywa",
                    ASSET_AMOUNT: "Kwota",
                    TYPE: "Typ stakingu",
                    SELECT_MAX: "MAX",
                    PERIOD_ASSETS: "Okres Staking",
                    ESTIMATED_APR: "Szacowany APR",
                    ESTIMATED_TOKEN: "Szacowana nagroda w tokenach",
                    ESTIMATED_USD: "Szacowana nagroda w USD"
                },
                WARNING: "Aktywacja Stakingu blokuje srodki na okreslony w kontrakcie czas. Wczesniejsze żądanie środków wiąże się z dodatkowymi opłatami.",
                SUBTITLE: {
                    AVALABLE_BALANCE: "Saldo",
                    STAKING_PERIOD: "Czas Staking"
                }
            },
            TOP_STAKING: {
                STAKING_TITLE: "Najlepsze tokeny",
                NO_TOP_STAKINGS: "Tutaj zobaczysz tokeny z najwyższym APR",
                ACTIONS: {
                    STAKE_NOW: "Aktywuj Staking",
                    BUY_TOKENS: "Kup tokeny"
                }
            },
            LEARN_MORE: "Dowiedz się więcej",
            VIEW_ALL: "Zobacz wszystko",
            PANEL: {
                TITLE: {
                    ACTIVE: "Aktywne",
                    STAKING_HISTORY: "Historia Staking"
                },
                ACTIVE: {
                    PAID_WALLET: "Płatność z portfela",
                    TITLE: {
                        NEXT_REWARD: "Następna nagroda",
                        STAKED_VALUE: "Wartość staking",
                        ESTIMATED_REWARD: 'Szacowana nagroda'
                    },
                    NO_TOKENS: "Tutaj zobaczysz swój aktywny staking",
                    ACTIONS: {
                        CLAIM_TOKENS: "Odbierz",
                        WITHDRAW: 'Zakończ'
                    }
                },
                HISTORY: {
                    TABLE: {
                        TOKEN_NAME: "Nazwa Tokena",
                        STATUS: "Status",
                        STAKED: "Wartość staking",
                        REWARDED: "Nagroda",
                        APR: "APR",
                        START_DATE: "Nagroda",
                        END_DATE: "Data końcowa"
                    },
                    NO_STAKINGS: "There are no stakings"
                }
            }
        },
        WALLET_VESTING: {
            TITLES: {
                CLAIMED: "Pobrano",
                TOTAL_PERIOD: "Całkowity okres",
                LOCKED: "Zablokowane",
                NEXT_UNLOCK: "Kolejne uwolnienie"
            },
            NO_VESTING: "Brak danych",
            ACTIONS: {
                CLAIM: "Pobierz",
                TOKENS: "tokenów"
            },
            SUCCESS: {
                TITLE: "Pobrałeś ",
                TOKENS: "tokenów.",
                DESCRIPTION: "Tokeny pojawią się na Twoim portfelu w ciągu kilku minut"
            }
        },
        WALLET_PAGE_MENU: {
            PORTFEL: 'Portfel'
        },
        USER_WALLET: {
            SUMMARY: {
                TOTAL: 'Aktywa ogółem',
                COPY_HINT: 'Skopiuj do schowka',
                ACTIONS: {
                    DEPOSIT: 'Wpłata',
                    SEND: 'Wyślij',
                    WITHDRAW: 'Wypłać',
                    MANUAL_DEPOSIT: 'Przelew ręczny'
                },
                MESSAGES: {
                  COPIED: "Skopiowano"
                }
            },
            OVERVIEW: {
                STAKING: {
                    TITLE: 'Alokowano środków',
                    COPY_HINT: 'Skopiuj do schowka',
                    REWARD_DATE_TITLE: 'Następna nagroda',
                    ACTIONS: {
                        VIEW_DETAILS: 'Wyświetl szczegóły',
                        DEPOSIT: 'Wpłata',
                        WITHDRAW: 'Wypłata'
                    },
                    MESSAGES: {
                    }
                },
                VESTING: {
                    TITLE: 'Zalokowano w vestingu',
                    COPY_HINT: 'Skopiuj do schowka',
                    REWARD_DATE_TITLE: 'Następna nagroda',
                    ACTIONS: {
                        VIEW_DETAILS: 'Wyświetl szczegóły'
                    },
                    MESSAGES: {
                    }
                },
                PACKAGES: {
                    TITLE: 'Zamknięty w paczkach',
                    COPY_HINT: 'Skopiuj do schowka',
                    ACTIVE_PACKAGES_TITLE: 'Aktywne pakiety',
                    ACTIONS: {
                        VIEW_DETAILS: 'Wyświetl szczegóły'
                    },
                    MESSAGES: {
                    }
                }
            },
            PANEL: {
                ASSETS: {
                    TITLE: 'Aktywa',
                    NO_ASSETS: 'Brak zasobów. Czas to zmienić...',
                    ACTIONS: {
                        EXCHANGE: 'Wymiana',
                        STAKE: 'Alokować',
                        MANAGE: 'Zarządzaj'
                    }
                },
                TRANSACTIONS: {
                    TITLE: 'Transakcje',
                    TABLE: {
                        TOKEN: 'Nazwa tokena',
                        HASH: 'Transakcja',
                        SOURCE: 'Źródło',
                        AMOUNT: 'Kwota',
                        DESTINATION: 'Przeznaczenie'
                    },
                    NO_TRANSACTIONS: 'Brak transakcji',
                    ACTIONS: {
                        LOAD_MORE: 'Wyświetl więcej'
                    }
                },
                KANGA_WALLET: {
                    TITLE: 'Portfel Kanga'
                }
            }
        },
        DISTRIBUTION: {
            MESSAGES: {
                STAGE_FAILED: 'Suma tokenów w etapach projektu przekracza całkowite zaopatrzenie tokenów.'
            },
            INVALID_TABLE: "Suma tokenów w etapach projektu przekracza całkowite zaopatrzenie tokenów."
        },
        STAKING_MODAL: {
            TITLE: 'Potwierdź wpłatę',
            CONTENT: 'Proszę o potwierdzenie wpłaty',
            DONE: 'Zgoda'
        },
        STAKING_WITHDRAW_MODAL: {
            TITLE: 'Zakończ stakowanie',
            CONTENT: 'Wycofujesz się ze stakingu.  Wycofanie się ze stakingu spowoduje przeniesienie wszystkich tokenów do Twojego portfela. Nie będziesz już kwalifikował się do następnej rundy nagród. Wszystkie punkty booster stakingu zostaną zredukowane do zera. Czy na pewno chcesz wycofać się ze stakingu?',
            DONE: 'Zgoda',
            WARNING: 'Uwaga! Ryzykujesz utratą całej nagrody za ostatni cykl.'
        },
        STAKING_REWARD_MODAL: {
            TITLE: 'Odbierz nagrodę',
            CONTENT: 'Otrzymasz przybliżoną nagrodę w wysokości ',
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
        },
        MODALS: {
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
            }
        }
    }
};








export enum LanguageEnum {
  EN = "en",
  PL = "pl",
}

export interface LanguageFlag {
    lang: string;
    name: string;
    flag: string;
    active?: boolean;
}

export const languages = [
    {
        lang: LanguageEnum.EN,
        name: 'ENG',
        flag: '/assets/media/flags/united-states.svg',
    },
    {
        lang: LanguageEnum.PL,
        name: 'PL',
        flag: '/assets/media/flags/poland.svg',
    }
];

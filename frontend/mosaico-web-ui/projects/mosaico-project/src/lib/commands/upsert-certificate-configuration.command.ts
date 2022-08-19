import {LogoBlock, TextBlock, TokensAmount} from "../models";

export interface UpsertCertificateConfigurationCommand {
    logo: LogoBlock;
    name: TextBlock;
    tokens: TokensAmount;
    date: TextBlock;
    code: TextBlock;
    sendingEnabled: boolean;
}

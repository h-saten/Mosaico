import {TextBlock} from "./certificate/text-block";
import {TokensAmount} from "./certificate/tokens-amount";
import {LogoBlock} from "./certificate/logo-block";

export interface CertificateConfiguration {
    investorName: TextBlock;
    tokensAmount: TokensAmount;
    date: TextBlock;
    code: TextBlock;
    logoBlock: LogoBlock;
}

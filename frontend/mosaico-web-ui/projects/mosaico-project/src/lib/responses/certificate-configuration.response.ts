import {CertificateConfiguration} from "../models";

export interface CertificateConfigurationResponse {
    configuration: CertificateConfiguration;
    backgroundUrl: string;
    hasBlocksConfiguration: boolean;
    hasConfiguration: boolean;
    sendCertificateToInvestor: boolean;
}

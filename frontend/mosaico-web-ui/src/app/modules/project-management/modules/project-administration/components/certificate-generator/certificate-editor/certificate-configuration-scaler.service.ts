import {Injectable} from '@angular/core';
import {CertificateConfiguration} from "mosaico-project";

@Injectable({
  providedIn: 'root'
})
export class CertificateConfigurationScalerService {

  constructor() {}

  scaleTo(inputConfiguration: CertificateConfiguration, scale: number): CertificateConfiguration {

    const investorName = inputConfiguration.investorName;
    investorName.marginTop = Math.round(investorName.marginTop * scale);
    investorName.marginLeft = Math.round(investorName.marginLeft * scale);
    investorName.width = Math.round(investorName.width * scale);
    investorName.height = Math.round(investorName.height * scale);
    investorName.fontSizePx = Math.round(investorName.fontSizePx * scale);

    const certificateDate = inputConfiguration.date;
    certificateDate.marginTop = Math.round(certificateDate.marginTop * scale);
    certificateDate.marginLeft = Math.round(certificateDate.marginLeft * scale);
    certificateDate.width = Math.round(certificateDate.width * scale);
    certificateDate.height = Math.round(certificateDate.height * scale);
    certificateDate.fontSizePx = Math.round(certificateDate.fontSizePx * scale);

    const tokensAmount = inputConfiguration.tokensAmount;
    tokensAmount.marginTop = Math.round(tokensAmount.marginTop * scale);
    tokensAmount.marginLeft = Math.round(tokensAmount.marginLeft * scale);
    tokensAmount.width = Math.round(tokensAmount.width * scale);
    tokensAmount.height = Math.round(tokensAmount.height * scale);
    tokensAmount.fontSizePx = Math.round(tokensAmount.fontSizePx * scale);

    const certificateNumber = inputConfiguration.code;
    certificateNumber.marginTop = Math.round(certificateNumber.marginTop * scale);
    certificateNumber.marginLeft = Math.round(certificateNumber.marginLeft * scale);
    certificateNumber.width = Math.round(certificateNumber.width * scale);
    certificateNumber.height = Math.round(certificateNumber.height * scale);
    certificateNumber.fontSizePx = Math.round(certificateNumber.fontSizePx * scale);

    const logoBlock = inputConfiguration.logoBlock;
    logoBlock.marginTop = Math.round(logoBlock.marginTop * scale);
    logoBlock.marginLeft = Math.round(logoBlock.marginLeft * scale);
    logoBlock.width = Math.round(logoBlock.width * scale);
    logoBlock.height = Math.round(logoBlock.height * scale);

    return {
      investorName: investorName,
      date: certificateDate,
      code: certificateNumber,
      tokensAmount: tokensAmount,
      logoBlock: logoBlock
    }
  }

  // scaleFromToOriginalSize(inputConfiguration: CertificateConfigurationData, fromScale: number): CertificateConfigurationData {
  //
  // }
}

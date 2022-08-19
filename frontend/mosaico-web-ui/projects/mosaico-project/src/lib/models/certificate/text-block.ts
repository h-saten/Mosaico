import {TextAlignType} from "./text-align.type";
import {BaseBlock} from "./base-block";

export interface TextBlock extends BaseBlock {
    fontSizePx: number;
    fontColor: string;
    textBold: boolean;
    textAlign: TextAlignType;
}

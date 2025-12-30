import { HAlignType } from "../types";
import ActionOption from "./ActionOption copy";

export default interface PositionedActionOption extends ActionOption {
    position?: HAlignType;
    block?: boolean;
}
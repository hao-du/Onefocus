import { JSX } from "react";
import { BaseProps } from "../../../props";

export interface PanelHeaderTemplateOptions extends BaseProps {
    titleElement: JSX.Element;
    togglerElement: JSX.Element;
}
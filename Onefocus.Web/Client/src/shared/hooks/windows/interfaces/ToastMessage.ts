import { ReactNode } from "react";
import { StateType } from "../../../types";

export default interface ToastMessage {
    severity?: StateType;
    title?: string;
    description?: string;
    icon?: ReactNode;
    canBeExpired?: boolean;
}
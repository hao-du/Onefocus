import React from "react";
import { IdentityProps, LabelProps } from "../props/BaseProps";

export interface FormOption extends IdentityProps, LabelProps {
    form?: React.ReactNode;
}
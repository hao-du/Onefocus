import { IconProps, IdentityProps, InteractionProps, LabelProps } from "../props/BaseProps";

export interface ActionOption extends IdentityProps, IconProps, LabelProps, InteractionProps {
    command?: () => void;
}
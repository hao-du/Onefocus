import {Button as PiButton} from "primereact/button";
import {BaseProps} from "../../props/BaseProps.ts";
import {MouseEventHandler} from "react";

type ButtonProps = BaseProps & {
    label?: string;
    icon?: string;
    disabled?: boolean;
    onClick:  MouseEventHandler<HTMLButtonElement>;
    isPending?: boolean;
};

const Button = (props : ButtonProps) => {
    return (
        <PiButton
            className={props.className}
            label={props.label}
            icon={props.isPending &&  props.icon ? 'pi pi-spin pi-spinner' : 'pi ' +  props.icon}
            disabled={props.disabled || props.isPending}
            onClick={props.onClick} />
    );
};

export default Button;
import {Button as PiButton} from 'primereact/button';
import {BaseButtonProps} from '../../../props/BaseButtonProps';

type ButtonProps = BaseButtonProps & {
    link?: boolean | undefined;
    type?: 'submit' | 'button' | undefined;
    iconPos?: 'top' | 'bottom' | 'left' | 'right' | undefined;
    "aria-haspopup"?: boolean | "false" | "true" | "menu" | "listbox" | "tree" | "grid" | "dialog" | undefined;
    "aria-controls"?: string | undefined;
};

export const Button = (props: ButtonProps) => {
    return (
        <PiButton
            type={props.type ? props.type : 'button'}
            className={props.className}
            label={props.label}
            icon={props.isPending && props.icon ? 'pi pi-spin pi-spinner' : 'pi ' + props.icon}
            disabled={props.disabled || props.isPending}
            onClick={props.onClick}
            link={props.link}
            iconPos={props.iconPos ?? 'left'}
            aria-haspopup={props['aria-controls']}
            aria-controls={props['aria-controls']}
        />
    );
};
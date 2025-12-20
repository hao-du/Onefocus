import { Button as PrimeButton } from 'primereact/button';
import { useLocale } from '../../../hooks/locale/LocaleContext';
import { MouseEventHandler } from 'react';
import { BaseHtmlProps, BaseIconProps, BaseIdentityProps } from '../../../props/BaseProps';
import { ImSpinner5 } from "react-icons/im";

type ButtonProps = BaseIdentityProps & BaseHtmlProps & BaseIconProps & {
    label?: string;
    disabled?: boolean;
    onClick?: MouseEventHandler<HTMLButtonElement>;
    isPending?: boolean;
    text?: boolean;
    rounded?: boolean;
    severity?: 'secondary' | 'success' | 'info' | 'warning' | 'danger' | 'help' | 'contrast';
    link?: boolean | undefined;
    type?: 'submit' | 'button' | undefined;
    iconPos?: 'top' | 'bottom' | 'left' | 'right' | undefined;
    "aria-haspopup"?: boolean | "false" | "true" | "menu" | "listbox" | "tree" | "grid" | "dialog" | undefined;
    "aria-controls"?: string | undefined;
};

export const Button = (props: ButtonProps) => {
    const { translate } = useLocale();

    return (
        <PrimeButton
            id={props.id}
            key={props.key}
            type={props.type ? props.type : 'button'}
            className={props.className}
            label={translate(props.label)}
            icon={props.isPending && props.icon ? <ImSpinner5 className='spin'/> : props.icon}
            disabled={props.disabled || props.isPending}
            onClick={props.onClick}
            link={props.link}
            iconPos={props.iconPos ?? 'left'}
            aria-haspopup={props['aria-haspopup']}
            aria-controls={props['aria-controls']}
            severity={props.severity}
            text={props.text}
            rounded={props.rounded}
        />
    );
};
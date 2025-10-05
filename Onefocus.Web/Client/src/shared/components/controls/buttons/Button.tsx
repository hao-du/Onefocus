import { Button as PrimeButton } from 'primereact/button';
import { BaseButtonProps } from '../../props';
import { useLocale } from '../../../hooks';

type ButtonProps = BaseButtonProps & {
    link?: boolean | undefined;
    type?: 'submit' | 'button' | undefined;
    iconPos?: 'top' | 'bottom' | 'left' | 'right' | undefined;
    "aria-haspopup"?: boolean | "false" | "true" | "menu" | "listbox" | "tree" | "grid" | "dialog" | undefined;
    "aria-controls"?: string | undefined;
    severity?: 'secondary' | 'success' | 'info' | 'warning' | 'danger' | 'help' | 'contrast' | undefined;
    text?: boolean;
    rounded?: boolean;
};

const Button = (props: ButtonProps) => {
    const { translate } = useLocale();

    return (
        <PrimeButton
            type={props.type ? props.type : 'button'}
            className={props.className}
            label={translate(props.label)}
            icon={props.isPending && props.icon ? 'pi pi-spin pi-spinner' : 'pi ' + props.icon}
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

export default Button;
import {Button as PiButton} from 'primereact/button';
import {BaseButtonProps} from '../../../props/BaseButtonProps';

type ButtonProps = BaseButtonProps & {
    link?: boolean | undefined;
    type?: 'submit' | 'button' | undefined;
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
            link={props.link}/>
    );
};
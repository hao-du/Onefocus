import { InputSwitch, InputSwitchChangeEvent } from 'primereact/inputswitch';
import { InputProps } from '../../../props/InputProps';

export type SwitchProps = InputProps & {
    checked: boolean;
    invalid?: boolean;
    onChange?(event: InputSwitchChangeEvent): void;
};

export const Switch = (props: SwitchProps) => {
    return (
        <>
            <label htmlFor={props.id}>{props.label}</label>
            <InputSwitch
                id={props.id}
                className={props.className}
                checked={props.checked}
                onChange={props.onChange}
                disabled={props.isPending || props.readOnly}
                invalid={props.invalid}
            />
        </>
    );
};
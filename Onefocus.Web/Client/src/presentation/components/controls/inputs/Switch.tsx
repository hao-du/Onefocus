import {InputSwitch, InputSwitchChangeEvent} from 'primereact/inputswitch';
import {InputProps} from '../../../props/InputProps';
import {InputWrapper} from './InputWrapper';

export type SwitchProps = InputProps & {
    checked: boolean;
    invalid?: boolean;
    onChange?(event: InputSwitchChangeEvent): void;
};

export const Switch = (props: SwitchProps) => {
    return (
        <InputWrapper {...props}>
            <InputSwitch
                id={props.id}
                className={`${props.className} p-inputwrapper-filled`}
                checked={props.checked}
                onChange={props.onChange}
                disabled={props.isPending || props.readOnly}
                invalid={props.invalid}
            />
        </InputWrapper>
    );
};
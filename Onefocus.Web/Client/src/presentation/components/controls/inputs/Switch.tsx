import {InputSwitch} from 'primereact/inputswitch';
import {InputProps} from '../../../props/InputProps';
import {InputWrapper} from './InputWrapper';

export type SwitchProps = InputProps & {
    checked: boolean;
    invalid?: boolean;
    onChange?: (value: boolean) => void;
    onValueChange?: (value: boolean) => void;
};

export const Switch = (props: SwitchProps) => {
    return (
        <InputWrapper {...props}>
            <InputSwitch
                id={props.id}
                className={`${props.className} p-inputwrapper-filled`}
                checked={props.checked}
                onChange={(e) => {
                    if(props.onValueChange) props.onValueChange(e.target.value);
                    if(props.onChange) props.onChange(e.target.value);
                }}
                disabled={props.isPending || props.readOnly}
                invalid={props.invalid}
            />
        </InputWrapper>
    );
};
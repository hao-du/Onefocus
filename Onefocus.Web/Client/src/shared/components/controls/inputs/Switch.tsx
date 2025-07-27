import { InputSwitch } from 'primereact/inputswitch';
import { InputProps } from '../../props';
import InputWrapper from './InputWrapper';

export type SwitchProps = InputProps & {
    checked?: boolean;
    invalid?: boolean;
    onValueChange?: (value: boolean) => void;
};

const Switch = (props: SwitchProps) => {
    return (
        <InputWrapper {...props}>
            <InputSwitch
                id={props.id}
                className={`${props.className} p-inputwrapper-filled`}
                checked={props.checked ?? false}
                onChange={(e) => {
                    if (props.onValueChange) props.onValueChange(e.target.value);
                }}
                disabled={props.isPending || props.readOnly}
                invalid={props.invalid}
            />
        </InputWrapper>
    );
};

export default Switch;
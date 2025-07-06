import {InputSwitch} from 'primereact/inputswitch';
import {InputProps} from '../../../props/InputProps';
import {InputWrapper} from './InputWrapper';
import React from 'react';

export type SwitchProps = InputProps & {
    checked: boolean;
    invalid?: boolean;
    onChange?: React.ChangeEvent<HTMLInputElement>;
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
                    if(props.onChange) props.onChange({
                        target: {
                            name: e.target.name,
                            value: e.target.value
                        }
                    });
                }}
                disabled={props.isPending || props.readOnly}
                invalid={props.invalid}
            />
        </InputWrapper>
    );
};
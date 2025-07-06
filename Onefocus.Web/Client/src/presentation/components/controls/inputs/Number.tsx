import {InputProps} from '../../../props/InputProps';
import {InputNumber} from 'primereact/inputnumber';
import {InputWrapper} from './InputWrapper';
import React from 'react';

export type NumberProps = InputProps & {
    name?: string;
    inputClassName?: string;
    value?: number;
    fractionDigits?: number
    textAlign?: 'right' | 'left';
    onChange?: React.ChangeEvent<HTMLInputElement>;
    onValueChange?: (value: number | null) => void;
};

export const Number = (props: NumberProps) => {
    return (
        <InputWrapper {...props}>
            <InputNumber
                name={props.name}
                className={props.className}
                id={props.id}
                value={props.value}
                onChange={(e) => {
                    if(props.onValueChange) props.onValueChange(e.value);
                    if(props.onChange) props.onChange({
                        target: {
                            name: e.originalEvent.target.name,
                            value: e.value
                        }
                    });
                }}
                invalid={props.invalid}
                readOnly={props.isPending || props.readOnly}
                minFractionDigits={props.fractionDigits}
                maxFractionDigits={props.fractionDigits}
                inputClassName={`${props.inputClassName ?? ''} text-${props.textAlign ?? 'right'}`}
                onKeyDown={(e) => {
                    if (e.key === 'Enter') {
                        e.preventDefault();
                    }
                }}
            />
        </InputWrapper>
    );
};
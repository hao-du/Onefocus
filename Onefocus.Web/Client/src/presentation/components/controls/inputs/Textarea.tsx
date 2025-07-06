import {InputTextarea} from 'primereact/inputtextarea';
import {InputProps} from '../../../props/InputProps';
import React, {ChangeEventHandler} from 'react';
import {InputWrapper} from './InputWrapper';

export type TextareaProps = InputProps & {
    rows?: number;
    floatClassName?: string;
    onChange?: React.ChangeEvent<HTMLInputElement>;
    onValueChange?: (value: string | null) => void;
};

export const Textarea = (props: TextareaProps) => {
    return (
        <InputWrapper {...props}>
            <InputTextarea
                className={props.className}
                id={props.id}
                value={props.value}
                onChange={(e) => {
                    if(props.onValueChange) props.onValueChange(e.target.value);
                    if(props.onChange) props.onChange({
                        target: {
                            name: e.target.name,
                            value: e.target.value
                        }
                    });
                }}
                readOnly={props.isPending || props.readOnly}
                rows={props.rows ?? 5}
                invalid={props.invalid}
            />
        </InputWrapper>
    );
};
import {InputText} from 'primereact/inputtext';
import {InputProps} from '../../../props/InputProps';
import {ChangeEventHandler} from 'react';
import {InputWrapper} from './InputWrapper';

export type TextProps = InputProps & {
    floatClassName?: string;
    onChange?: ChangeEventHandler<HTMLInputElement>;
    autoComplete?: 'username' | string;
};

export const Text = (props: TextProps) => {
    return (
        <InputWrapper {...props}>
            <InputText
                className={props.className}
                id={props.id}
                value={props.value}
                onChange={props.onChange}
                invalid={props.invalid}
                readOnly={props.isPending || props.readOnly}
                autoComplete={props.autoComplete}
            />
        </InputWrapper>
    );
};
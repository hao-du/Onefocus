import {Password as PiPassword} from 'primereact/password';
import {InputProps} from '../../../props/InputProps';
import {ChangeEventHandler} from 'react';
import {InputWrapper} from './InputWrapper';

export type PasswordProps = InputProps & {
    floatClassName?: string;
    feedback?: boolean;
    onChange?: ChangeEventHandler<HTMLInputElement>;
};

export const Password = (props: PasswordProps) => {
    return (
        <InputWrapper {...props}>
            <PiPassword
                inputClassName={props.className}
                inputId={props.id}
                feedback={props.feedback ?? false}
                value={props.value} onChange={props.onChange}
                readOnly={props.isPending || props.readOnly}
                invalid={props.invalid}
                autoComplete="current-password"
            />
        </InputWrapper>
    );
};
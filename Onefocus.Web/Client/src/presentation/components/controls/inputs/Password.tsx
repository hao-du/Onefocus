import {Password as PiPassword} from 'primereact/password';
import {InputProps} from '../../../props/InputProps';
import {InputWrapper} from './InputWrapper';

export type PasswordProps = InputProps & {
    floatClassName?: string;
    feedback?: boolean;
    onChange?: (value: string | null) => void;
    onValueChange?: (value: string | null) => void;
};

export const Password = (props: PasswordProps) => {
    return (
        <InputWrapper {...props}>
            <PiPassword
                inputClassName={props.className}
                inputId={props.id}
                feedback={props.feedback ?? false}
                value={props.value}
                onChange={(e) => {
                    if(props.onValueChange) props.onValueChange(e.target.value);
                    if(props.onChange) props.onChange(e.target.value);
                }}
                readOnly={props.isPending || props.readOnly}
                invalid={props.invalid}
                autoComplete="current-password"
            />
        </InputWrapper>
    );
};
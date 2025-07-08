import {InputText} from 'primereact/inputtext';
import {InputProps} from '../../../props/InputProps';
import {InputWrapper} from './InputWrapper';

export type TextProps = InputProps & {
    floatClassName?: string;
    autoComplete?: 'username' | string;
    onChange?: (value: string | null) => void;
    onValueChange?: (value: string | null) => void;
};

export const Text = (props: TextProps) => {
    return (
        <InputWrapper {...props}>
            <InputText
                className={props.className}
                id={props.id}
                value={props.value}
                onChange={(e) => {
                    if(props.onValueChange) props.onValueChange(e.target.value);
                    if(props.onChange) props.onChange(e.target.value);
                }}
                invalid={props.invalid}
                readOnly={props.isPending || props.readOnly}
                autoComplete={props.autoComplete}
            />
        </InputWrapper>
    );
};
import {InputTextarea} from 'primereact/inputtextarea';
import {InputProps} from '../../../props/InputProps';
import {ChangeEventHandler} from 'react';
import {InputWrapper} from './InputWrapper';

export type TextareaProps = InputProps & {
    rows?: number;
    floatClassName?: string;
    onChange?: ChangeEventHandler<HTMLTextAreaElement>;
};

export const Textarea = (props: TextareaProps) => {
    return (
        <InputWrapper {...props}>
            <InputTextarea
                className={props.className}
                id={props.id}
                value={props.value}
                onChange={props.onChange}
                readOnly={props.isPending || props.readOnly}
                rows={props.rows ?? 5}
                invalid={props.invalid}
            />
        </InputWrapper>
    );
};
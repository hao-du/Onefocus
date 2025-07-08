import {InputTextarea} from 'primereact/inputtextarea';
import {InputProps} from '../../../props/InputProps';
import {InputWrapper} from './InputWrapper';

export type TextareaProps = InputProps & {
    rows?: number;
    floatClassName?: string;
    onChange?: (value: string | null) => void;
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
                    if(props.onChange) props.onChange(e.target.value);
                }}
                readOnly={props.isPending || props.readOnly}
                rows={props.rows ?? 5}
                invalid={props.invalid}
            />
        </InputWrapper>
    );
};
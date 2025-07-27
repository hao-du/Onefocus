import { InputTextarea } from 'primereact/inputtextarea';
import { InputProps } from '../../props';
import InputWrapper from './InputWrapper';

export type TextareaProps = InputProps & {
    rows?: number;
    floatClassName?: string;
    onValueChange?: (value: string | null) => void;
};

const Textarea = (props: TextareaProps) => {
    return (
        <InputWrapper {...props}>
            <InputTextarea
                className={props.className}
                id={props.id}
                value={props.value}
                onChange={(e) => {
                    if (props.onValueChange) props.onValueChange(e.target.value);
                }}
                readOnly={props.isPending || props.readOnly}
                rows={props.rows ?? 5}
                invalid={props.invalid}
            />
        </InputWrapper>
    );
};

export default Textarea;
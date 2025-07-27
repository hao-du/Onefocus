import { InputText } from 'primereact/inputtext';
import { InputProps } from '../../props';
import InputWrapper from './InputWrapper';

export type TextProps = InputProps & {
    floatClassName?: string;
    autoComplete?: 'username' | string;
    onValueChange?: (value: string | null) => void;
};

const
    Text = (props: TextProps) => {
        return (
            <InputWrapper {...props}>
                <InputText
                    className={props.className}
                    id={props.id}
                    value={props.value}
                    onChange={(e) => {
                        if (props.onValueChange) props.onValueChange(e.target.value);
                    }}
                    invalid={props.invalid}
                    readOnly={props.isPending || props.readOnly}
                    autoComplete={props.autoComplete}
                />
            </InputWrapper>
        );
    };

export default Text;
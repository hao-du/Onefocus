import { InputProps } from '../../props';
import { InputNumber } from 'primereact/inputnumber';
import InputWrapper from './InputWrapper';

export type NumberProps = InputProps & {
    name?: string;
    inputClassName?: string;
    value?: number;
    fractionDigits?: number
    textAlign?: 'right' | 'left';
    onValueChange?: (value: number | null) => void;
};

const Number = (props: NumberProps) => {
    return (
        <InputWrapper {...props}>
            <InputNumber
                name={props.name}
                className={props.className}
                id={props.id}
                value={props.value}
                onChange={(e) => {
                    if (props.onValueChange) props.onValueChange(e.value);
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

export default Number;
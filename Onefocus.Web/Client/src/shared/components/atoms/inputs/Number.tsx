import { InputNumber } from "primereact/inputnumber";
import { BaseHtmlProps, BaseIdentityProps } from "../../../props/BaseProps";

export interface NumberProps extends BaseIdentityProps, BaseHtmlProps {
    value?: number;
    readOnly?: boolean;
    isPending?: boolean;
    invalid?: boolean;
    placeholder?: string;
    size?: 'normal' | 'small',
    onChange?: ((value: number | null) => void);
    feedback?: boolean;
    fractionDigits?: number;
    textAlign?: 'right' | 'left';
    inputClassName?: string;
};

export const Number = (props: NumberProps) => {
    return (
        <InputNumber
            id={props.id}
            key={props.key}
            onChange={(e) => {
                if (props.onChange) {
                    props.onChange(e.value);
                }
            }}
            invalid={props.invalid}
            value={props.value}
            readOnly={props.isPending || props.readOnly}
            minFractionDigits={props.fractionDigits}
            maxFractionDigits={props.fractionDigits}
            onKeyDown={(e) => {
                if (e.key === 'Enter') {
                    e.preventDefault();
                }
            }}
            className={`${props.className} ${props.size == 'small' ? 'p-inputtext-sm' : ''}`}
            inputClassName={`${props.inputClassName ?? ''} text-${props.textAlign ?? 'right'}`}
            placeholder={props.placeholder}
        />
    );
};
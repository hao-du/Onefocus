import {InputProps} from '../../../props/InputProps';
import {InputNumber, InputNumberValueChangeEvent} from 'primereact/inputnumber';

export type NumberProps = InputProps & {
    inputClassName?: string;
    onChange?: ((event: InputNumberValueChangeEvent) => void);
    value?: number;
    fractionDigits?: number
    textAlign?: 'right' | 'left';
};

export const Number = (props: NumberProps) => {
    return (
        <div className="flex flex-column gap-2 mb-3">
            <label className={props.invalid ? 'p-error' : ''} htmlFor={props.id}>{props.label}</label>
            <InputNumber
                className={props.className}
                id={props.id}
                value={props.value}
                onValueChange={props.onChange}
                invalid={props.invalid}
                readOnly={props.isPending || props.readOnly}
                minFractionDigits={props.fractionDigits}
                maxFractionDigits={props.fractionDigits}
                inputClassName={`${props.inputClassName ?? ''} text-${props.textAlign ?? 'right'}`}
            />
            {props.description && (<small className="of-text-200">{props.description}</small>)}
            {props.errorMessage && (<small className="p-error">{props.errorMessage}</small>)}
        </div>
    );
};
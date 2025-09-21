import { Controller, FieldPath, FieldValues, UseControllerProps } from "react-hook-form";
import InputWrapper from "./InputWrapper";
import { InputProps, InputWrapperProps } from "../../props";
import { InputNumber } from "primereact/inputnumber";

type NumberProps<TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    = UseControllerProps<TFieldValues, TName, TTransformedValues>
    & InputProps
    & InputWrapperProps
    & {
        value?: number;
        feedback?: boolean;
        fractionDigits?: number;
        textAlign?: 'right' | 'left';
        inputClassName?: string;
    };

const Number = <TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    (props: NumberProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller
            name={props.name}
            control={props.control}
            rules={props.textOnly ? undefined : props.rules}
            render={(controller) => {
                return (
                    <InputWrapper
                        {...props}
                        htmlFor={controller.field.name}
                        errorMessage={controller.fieldState.error?.message}
                    >
                        {props.textOnly
                            ? <p>{controller.field.value}</p>
                            : <InputNumber
                                id={controller.field.name}
                                onChange={(e) => { controller.field.onChange(e.value); }}
                                invalid={controller.fieldState.invalid}
                                value={controller.field.value}
                                minFractionDigits={props.fractionDigits}
                                maxFractionDigits={props.fractionDigits}
                                readOnly={props.isPending || props.readOnly}
                                onKeyDown={(e) => {
                                    if (e.key === 'Enter') {
                                        e.preventDefault();
                                    }
                                }}
                                className={`${props.className} ${props.size == 'small' ? 'p-inputtext-sm' : ''}`}
                                inputClassName={`${props.inputClassName ?? ''} text-${props.textAlign ?? 'right'}`}
                            />
                        }
                    </InputWrapper>);
            }}
        />
    );
};

export default Number;
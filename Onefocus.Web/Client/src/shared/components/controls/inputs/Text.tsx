import { Controller, FieldPath, FieldValues, UseControllerProps } from "react-hook-form";
import InputWrapper from "./InputWrapper";
import { InputText } from "primereact/inputtext";
import { InputProps, InputWrapperProps } from "../../props";

type TextProps<TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    = UseControllerProps<TFieldValues, TName, TTransformedValues>
    & InputProps
    & InputWrapperProps
    & {
        autoComplete?: 'username' | string;
        keyfilter?: 'email' | 'int';
    };

const Text = <TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    (props: TextProps<TFieldValues, TName, TTransformedValues>) => {
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
                            : <InputText
                                id={controller.field.name}
                                onChange={(e) => { controller.field.onChange(e.target.value); }}
                                invalid={controller.fieldState.invalid}
                                value={controller.field.value}
                                readOnly={props.isPending || props.readOnly}
                                autoComplete={props.autoComplete}
                                className={`${props.className} ${props.size == 'small' ? 'p-inputtext-sm' : ''}`}
                                keyfilter={props.keyfilter}
                                placeholder={props.placeholder}
                            />
                        }
                    </InputWrapper>
                );
            }}
        />
    );
};

export default Text;
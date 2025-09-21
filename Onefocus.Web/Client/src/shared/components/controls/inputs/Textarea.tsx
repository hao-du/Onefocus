import { Controller, FieldPath, FieldValues, UseControllerProps } from "react-hook-form";
import InputWrapper from "./InputWrapper";
import { InputProps, InputWrapperProps } from "../../props";
import { InputTextarea } from "primereact/inputtextarea";

type TextareaProps<TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    = UseControllerProps<TFieldValues, TName, TTransformedValues>
    & InputProps
    & InputWrapperProps
    & {
        rows?: number;
    };

const Textarea = <TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    (props: TextareaProps<TFieldValues, TName, TTransformedValues>) => {
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
                            : <InputTextarea
                                id={controller.field.name}
                                onChange={(e) => { controller.field.onChange(e.target.value); }}
                                invalid={controller.fieldState.invalid}
                                value={controller.field.value}
                                rows={props.rows ?? 5}
                                readOnly={props.isPending || props.readOnly}
                                className={`${props.className} ${props.size == 'small' ? 'p-inputtext-sm' : ''}`}
                            />
                        }
                    </InputWrapper>);
            }}
        />
    );
};

export default Textarea;
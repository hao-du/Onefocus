import { InputText } from "primereact/inputtext";
import { BaseHtmlProps, BaseIdentityProps } from "../../../props/BaseProps";

export interface TextProps extends BaseIdentityProps, BaseHtmlProps {
    value?: string;
    readOnly?: boolean;
    isPending?: boolean;
    invalid?: boolean;
    placeholder?: string;
    size?: 'normal' | 'small',
    onChange?: ((value: string) => void);
    autoComplete?: 'username' | string;
    keyfilter?: 'email' | 'int';
}

export const Text = (props: TextProps) => {
    return (
        <InputText
            id={props.id}
            key={props.key}
            onChange={(e) => {
                if (props.onChange) {
                    props.onChange(e.target.value);
                }
            }}
            invalid={props.invalid}
            value={props.value}
            readOnly={props.isPending || props.readOnly}
            autoComplete={props.autoComplete ?? 'off'}
            className={`${props.className} ${props.size == 'small' ? 'p-inputtext-sm' : ''}`}
            keyfilter={props.keyfilter}
            placeholder={props.placeholder}
        />
    );
};
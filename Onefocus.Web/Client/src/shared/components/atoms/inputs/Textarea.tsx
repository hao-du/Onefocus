import { InputTextarea } from "primereact/inputtextarea";
import { BaseHtmlProps, BaseIdentityProps } from "../../../props/BaseProps";

export interface TextProps extends BaseIdentityProps, BaseHtmlProps {
    value?: string;
    readOnly?: boolean;
    isPending?: boolean;
    invalid?: boolean;
    placeholder?: string;
    size?: 'normal' | 'small',
    onChange?: ((value: string) => void);
    rows?: number;
}

export const Textarea = (props: TextProps) => {
    return (
        <InputTextarea
            id={props.id}
            key={props.key}
            onChange={(e) => {
                if (props.onChange) {
                    props.onChange(e.target.value);
                }
            }}
            invalid={props.invalid}
            value={props.value}
            rows={props.rows ?? 5}
            readOnly={props.isPending || props.readOnly}
            className={`${props.className} ${props.size == 'small' ? 'p-inputtext-sm' : ''}`}
        />
    );
};
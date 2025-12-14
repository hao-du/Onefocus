import { Password as PrimePassword } from 'primereact/password';
import { BaseHtmlProps, BaseIdentityProps } from "../../../props/BaseProps";

export interface PasswordProps extends BaseIdentityProps, BaseHtmlProps {
    value?: string;
    readOnly?: boolean;
    isPending?: boolean;
    invalid?: boolean;
    placeholder?: string;
    size?: 'normal' | 'small',
    onChange?: ((value: string) => void);
    autoComplete?: 'current-password' | string;
    feedback?: boolean;
};

export const Password = (props: PasswordProps) => {
    return (
        <PrimePassword
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
            autoComplete={props.autoComplete ?? 'current-password'}
            feedback={props.feedback ?? false}
            inputClassName={`${props.className} ${props.size == 'small' ? 'p-inputtext-sm' : ''}`}
            placeholder={props.placeholder}
        />
    );
};
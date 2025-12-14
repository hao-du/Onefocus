import { InputSwitch } from "primereact/inputswitch";
import { BaseHtmlProps, BaseIdentityProps } from "../../../props/BaseProps";

export interface SwitchProps extends BaseIdentityProps, BaseHtmlProps {
    value?: boolean;
    readOnly?: boolean;
    isPending?: boolean;
    invalid?: boolean;
    size?: 'normal' | 'small',
    onChange?: ((value: boolean) => void);
};

export const Switch = (props: SwitchProps) => {
    return (
        <InputSwitch
            id={props.id}
            key={props.key}
            onChange={(e) => {
                if (props.onChange) {
                    props.onChange(e.target.value);
                }
            }}
            invalid={props.invalid}
            checked={props.value ?? false}
            disabled={props.isPending || props.readOnly}
            className={`${props.className} ${props.size == 'small' ? 'p-inputswitch-sm' : ''}`}
        />
    );
};
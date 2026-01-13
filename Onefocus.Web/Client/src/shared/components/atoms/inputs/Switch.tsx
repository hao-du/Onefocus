import { Switch as AntSwitch } from "antd";
import { ClassNameProps, FocusProps, IdentityProps, InteractionProps, NameProps } from "../../../props/BaseProps";

export interface SwitchProps extends ClassNameProps, IdentityProps, InteractionProps, NameProps, FocusProps {
    value?: boolean;
    checkedLabel?: string;
    uncheckedLabel?: string;
    onChange?: (value: boolean) => void;
}

const Switch = (props: SwitchProps) => {
    return (
        <AntSwitch
            key={props.key}
            id={props.id}
            value={props.value}
            className={props.className}
            disabled={props.disabled || props.isPending}
            checkedChildren={props.checkedLabel}
            unCheckedChildren={props.uncheckedLabel}
            onChange={(e) => {
                if (props.onChange) props.onChange(e);
            }}
        />
    );
};

export default Switch;
import { Input } from "antd";
import { ClassNameProps, IdentityProps, InteractionProps } from "../../../props/BaseProps";
import { SizeType } from "antd/es/config-provider/SizeContext";
import { StateType } from "../../../types";

export interface PasswordProps extends ClassNameProps, IdentityProps, InteractionProps {
    placeHolder?: string;
    size?: SizeType;
    defaultValue?: string | number | readonly string[];
    status?: Exclude<StateType, 'info'> | 'validating';
    onChange?: (value: string) => void;
}

const Password = (props: PasswordProps) => {
    return (
        <Input.Password
            key={props.key}
            id={props.id}
            className={props.className}
            size={props.size}
            defaultValue={props.defaultValue}
            status={props.status}
            disabled={props.disabled || props.isPending}
            onChange={(e) => {
                e.preventDefault();
                if (props.onChange) props.onChange(e.target.value);
            }}
        />
    );
};

export default Password;
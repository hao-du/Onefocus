import { Input as AntInput } from "antd";
import { ClassNameProps, IdentityProps, InteractionProps, NameProps } from "../../../props/BaseProps";
import { SizeType } from "antd/es/config-provider/SizeContext";
import { StateType } from "../../../types";

export interface TextProps extends ClassNameProps, IdentityProps, InteractionProps, NameProps {
    autoComplete?: string;
    placeHolder?: string;
    size?: SizeType;
    defaultValue?: string | number | readonly string[];
    status?: Exclude<StateType, 'info'> | 'validating';
    onChange?: (value: string) => void;
}

const TextInput = (props: TextProps) => {
    return (
        <AntInput
            key={props.key}
            id={props.id}
            name={props.name}
            className={props.className}
            size={props.size}
            defaultValue={props.defaultValue}
            status={props.status}
            disabled={props.disabled || props.isPending}
            autoComplete={props.autoComplete}
            onFocus={(e) => e.target.select()}
            onChange={(e) => {
                e.preventDefault();
                if (props.onChange) props.onChange(e.target.value);
            }}
        />
    );
};

export default TextInput;
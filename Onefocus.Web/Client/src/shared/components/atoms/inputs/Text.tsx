import { Input as AntInput } from "antd";
import { ClassNameProps, FocusProps, IdentityProps, InteractionProps, NameProps } from "../../../props/BaseProps";
import { SizeType } from "antd/es/config-provider/SizeContext";
import { StateType } from "../../../types";
import { joinClassNames } from "../../../utils";

export interface TextProps extends ClassNameProps, IdentityProps, InteractionProps, NameProps, FocusProps {
    autoComplete?: string;
    placeHolder?: string;
    size?: SizeType;
    defaultValue?: string | number | readonly string[];
    status?: Exclude<StateType, 'info'> | 'validating';
    onChange?: (value: string) => void;
    value?: string;
}

const TextInput = (props: TextProps) => {
    return (
        <AntInput
            key={props.key}
            id={props.id}
            name={props.name}
            value={props.value}
            className={joinClassNames('w-full', props.className)}
            size={props.size}
            defaultValue={props.defaultValue}
            status={props.status}
            disabled={props.disabled || props.isPending}
            autoComplete={props.autoComplete}
            placeholder={props.placeHolder}
            onFocus={(e) => { if (props.focus) e.target.select(); }}
            onChange={(e) => {
                e.preventDefault();
                if (props.onChange) props.onChange(e.target.value);
            }}
        />
    );
};

export default TextInput;
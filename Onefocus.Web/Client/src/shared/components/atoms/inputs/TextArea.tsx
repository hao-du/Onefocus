import { Input } from "antd";
import { ClassNameProps, FocusProps, IdentityProps, InteractionProps, NameProps, ReadOnlyProps } from "../../../props/BaseProps";
import { SizeType } from "antd/es/config-provider/SizeContext";
import { InputVariantType, StateType } from "../../../types";
import { joinClassNames } from "../../../utils";

export interface TextAreaProps extends ClassNameProps, IdentityProps, InteractionProps, NameProps, FocusProps, ReadOnlyProps {
    autoComplete?: string;
    placeHolder?: string;
    size?: SizeType;
    defaultValue?: string | number | readonly string[];
    status?: Exclude<StateType, 'info'> | 'validating';
    onChange?: (value: string) => void;
    rows?: number;
    value?: string;
    variant?: InputVariantType;
}

const TextArea = (props: TextAreaProps) => {
    return (
        <Input.TextArea
            key={props.key}
            id={props.id}
            name={props.name}
            value={props.value}
            className={joinClassNames('w-full', props.className)}
            style={{ resize: 'none' }}
            rows={props.rows ?? 3}
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
            variant={props.variant ?? props.readOnly ? 'underlined' : undefined}
            readOnly={props.readOnly}
        />
    );
};

export default TextArea;
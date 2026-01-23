import { InputNumber } from "antd";
import { ClassNameProps, FocusProps, IdentityProps, InteractionProps, NameProps, ReadOnlyProps } from "../../../props/BaseProps";
import { SizeType } from "antd/es/config-provider/SizeContext";
import { InputVariantType, StateType } from "../../../types";
import { joinClassNames } from "../../../utils";

export interface NumberProps extends ClassNameProps, IdentityProps, InteractionProps, NameProps, FocusProps, ReadOnlyProps {
    autoComplete?: string;
    placeHolder?: string;
    size?: SizeType;
    defaultValue?: string | number | readonly string[];
    status?: Exclude<StateType, 'info'> | 'validating';
    onChange?: (value: string | null) => void;
    value?: string;
    formatted?: boolean;
    precision?: number;
    variant?: InputVariantType;
}

const Number = (props: NumberProps) => {
    return (
        <InputNumber
            key={props.key}
            id={props.id}
            name={props.name}
            value={props.value}
            style={{ width: '100%' }}
            formatter={props.formatted ? (value) => {
                const [start, end] = `${value}`.split('.') || [];
                const v = `${start}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                return `${end ? `${v}.${end}` : `${v}`}`;
            } : undefined}
            precision={props.precision}
            parser={props.formatted ? (value) => value?.replace(/\$\s?|(,*)/g, '') ?? '' : undefined}
            controls={false}
            className={joinClassNames(props.formatted ? 'text-right' : 'text-left', props.className)}
            size={props.size}
            status={props.status}
            disabled={props.disabled || props.isPending}
            autoComplete={props.autoComplete}
            placeholder={props.placeHolder}
            onFocus={(e) => { if (props.focus) e.target.select(); }}
            onChange={(value) => {
                if (props.onChange) props.onChange(value);
            }}
            variant={props.variant ?? props.readOnly ? 'underlined' : undefined}
            readOnly={props.readOnly}
            autoFocus={props.focus}
        />
    );
};

export default Number;
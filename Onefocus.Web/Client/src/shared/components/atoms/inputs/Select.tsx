import { Select as AntSelect } from 'antd';
import { ClassNameProps, FocusProps, IdentityProps, InteractionProps, NameProps } from "../../../props/BaseProps";
import { SizeType } from "antd/es/config-provider/SizeContext";
import { StateType } from "../../../types";
import { joinClassNames } from '../../../utils';
import { SelectOption } from '../../../options/SelectOption';

export interface SelectProps extends ClassNameProps, IdentityProps, InteractionProps, NameProps, FocusProps {
    placeHolder?: string;
    size?: SizeType;
    status?: Exclude<StateType, 'info'> | 'validating';
    onChange?: (value: string | null) => void;
    value?: string;
    loading?: boolean;
    options?: SelectOption[];
    mode?: 'multiple' | 'tags';
}

const Select = (props: SelectProps) => {
    return (
        <AntSelect
            key={props.key}
            id={props.id}
            value={props.value}
            mode={props.mode}
            className={joinClassNames('w-full', props.className)}
            showSearch={{
                filterOption: (input, option) =>
                    String(option?.label ?? '').toLowerCase().includes(input.toLowerCase()),
            }}
            loading={props.loading}
            options={props.options}
            size={props.size}
            status={props.status}
            disabled={props.disabled || props.isPending}
            placeholder={props.placeHolder}
            onChange={(value) => {
                if (props.onChange) props.onChange(value);
            }}
        />
    );
};

export default Select;
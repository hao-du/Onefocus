import { Dropdown as PiDropdown } from 'primereact/dropdown';
import { ReactNode } from 'react';
import { InputProps } from '../../props';
import { Option } from '../interfaces';

export type DropdownProps = InputProps & {
    options: Option[];
    placeholder?: string;
    className?: string;
    autoComplete?: 'on' | 'off' | string;
    disabled?: boolean;
    value?: string | number | boolean | null;
    onValueChange?: (value: string | number | boolean | null) => void;
    itemTemplate?: (option: Option) => ReactNode;
};

const Dropdown = (props: DropdownProps) => {
    return (
        <PiDropdown
            id={props.id}
            className={props.className}
            value={props.value}
            options={props.options}
            onChange={(e) => {
                if (props.onValueChange) props.onValueChange(e.target.value);
            }}
            disabled={props.isPending || props.disabled}
            autoComplete={props.autoComplete}
            placeholder={props.placeholder}
            itemTemplate={props.itemTemplate}
            data-testid={props.id}
            filter
            checkmark={true}
            highlightOnSelect={true}
            showClear
            invalid={props.invalid}
        />
    );
};

export default Dropdown;
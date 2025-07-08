import { Dropdown as PiDropdown} from 'primereact/dropdown';
import { InputProps } from '../../../props/InputProps';
import { ReactNode } from 'react';
import { InputWrapper } from './InputWrapper';

export type DropdownOption = {
    label: string;
    value: string | number | boolean;
};

export type DropdownProps = InputProps & {
    options: DropdownOption[];
    placeholder?: string;
    className?: string;
    autoComplete?: 'on' | 'off' | string;
    disabled?: boolean;
    value?: string | number | boolean | null;
    onChange?: (value: string | number | boolean | null) => void;
    onValueChange?: (value: string | number | boolean | null) => void;
    itemTemplate?: (option: DropdownOption) => ReactNode;
};

export const Dropdown = (props: DropdownProps) => {
    return (
        <InputWrapper {...props}>
            <PiDropdown
                id={props.id}
                className={props.className}
                value={props.value}
                options={props.options}
                onChange={(e) => {
                    if(props.onValueChange) props.onValueChange(e.target.value);
                    if(props.onChange) props.onChange(e.target.value);
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
        </InputWrapper>
    );
};
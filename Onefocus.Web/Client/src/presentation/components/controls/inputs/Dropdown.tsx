import { Dropdown as PiDropdown} from 'primereact/dropdown';
import { DropdownChangeEvent } from 'primereact/dropdown';
import { InputProps } from '../../../props/InputProps';
import { ReactNode } from 'react';

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
    onChange?: (e: DropdownChangeEvent) => void;
    itemTemplate?: (option: DropdownOption) => ReactNode;
};

export const Dropdown = (props: DropdownProps) => {
    return (
        <div className="flex flex-column gap-2 mb-3">
            <label className={props.invalid ? 'p-error' : ''} htmlFor={props.id}>
                {props.label}
            </label>
            <PiDropdown
                id={props.id}
                className={props.className}
                value={props.value}
                options={props.options}
                onChange={props.onChange}
                disabled={props.isPending || props.disabled}
                autoComplete={props.autoComplete}
                placeholder={props.placeholder}
                itemTemplate={props.itemTemplate}
                data-testid={props.id}
            />
            {props.description && <small className="of-text-200">{props.description}</small>}
            {props.errorMessage && <small className="p-error">{props.errorMessage}</small>}
        </div>
    );
};
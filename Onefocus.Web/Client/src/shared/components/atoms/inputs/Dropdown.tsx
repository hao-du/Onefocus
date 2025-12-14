import { ReactNode } from 'react';
import { Dropdown as PrimeDropdown } from 'primereact/dropdown';
import { BaseHtmlProps, BaseIdentityProps } from '../../../props/BaseProps';
import { useLocale } from '../../../hooks/locale/LocaleContext';
import { Option } from '../options/Option';

export interface DropdownProps extends BaseIdentityProps, BaseHtmlProps {
    value?: string | number | boolean | null;
    readOnly?: boolean;
    isPending?: boolean;
    invalid?: boolean;
    disabled?: boolean;
    placeholder?: string;
    size?: 'normal' | 'small',
    onChange?: ((value: string | number | boolean) => void);
    options: Option[];
    autoComplete?: 'on' | 'off' | string;
    itemTemplate?: (option: Option) => ReactNode;
    filter?: boolean;
    emptyMessage?: string;
};

export const Dropdown = (props: DropdownProps) => {
    const { translate } = useLocale();

    return (
        <PrimeDropdown
            id={props.id}
            key={props.key}
            onChange={(e) => {
                if (props.onChange) {
                    props.onChange(e.value);
                }
            }}
            invalid={props.invalid}
            value={props.value}
            options={props.options}
            itemTemplate={props.itemTemplate}
            placeholder={props.placeholder}
            readOnly={props.isPending || props.readOnly}
            disabled={props.isPending || props.disabled}
            autoComplete={props.autoComplete}
            filter={props.filter ?? true}
            checkmark={true}
            highlightOnSelect={true}
            className={`${props.className} ${props.size == 'small' ? 'p-inputtext-sm' : ''}`}
            emptyMessage={props.emptyMessage ?? translate('Nothing to show right now.')}
        />
    );
};
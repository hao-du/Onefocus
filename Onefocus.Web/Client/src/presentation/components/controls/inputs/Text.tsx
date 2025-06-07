import {InputText} from 'primereact/inputtext';
import {InputProps} from '../../../props/InputProps';
import {ChangeEventHandler} from 'react';

export type TextProps = InputProps & {
    floatClassName?: string;
    onChange?: ChangeEventHandler<HTMLInputElement>;
    autoComplete?: 'username' | string;
};

export const Text = (props: TextProps) => {
    return (
        <div className="flex flex-column gap-2 mb-3">
            <label className={props.invalid ? 'p-error' : ''} htmlFor={props.id}>{props.label}</label>
            <InputText
                className={props.className}
                id={props.id}
                value={props.value}
                onChange={props.onChange}
                invalid={props.invalid}
                readOnly={props.isPending || props.readOnly}
                autoComplete={props.autoComplete}
            />
            {props.description && (<small className="of-text-200">{props.description}</small>)}
            {props.errorMessage && (<small className="p-error">{props.errorMessage}</small>)}
        </div>
    );
};
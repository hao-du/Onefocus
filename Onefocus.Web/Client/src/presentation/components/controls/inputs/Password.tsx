import {Password as PiPassword} from 'primereact/password';
import {InputProps} from '../../../props/InputProps';
import {ChangeEventHandler} from 'react';

export type PasswordProps = InputProps & {
    floatClassName?: string;
    feedback?: boolean;
    onChange?: ChangeEventHandler<HTMLInputElement>;
};

export const Password = (props: PasswordProps) => {
    return (
        <div className="flex flex-column gap-2 mb-3">
            <label className={props.invalid ? 'p-error' : ''} htmlFor={props.id}>{props.label}</label>
            <PiPassword
                inputClassName={props.className}
                inputId={props.id}
                feedback={props.feedback ?? false}
                value={props.value} onChange={props.onChange}
                readOnly={props.isPending || props.readOnly}
                invalid={props.invalid}
                autoComplete="current-password"
            />
            {props.description && (<small>{props.description}</small>)}
            {props.errorMessage && (<small className="p-error">{props.errorMessage}</small>)}
        </div>
    );
};
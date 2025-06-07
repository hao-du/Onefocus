import {InputTextarea} from 'primereact/inputtextarea';
import {InputProps} from '../../../props/InputProps';
import {ChangeEventHandler} from 'react';

export type TextareaProps = InputProps & {
    rows?: number;
    floatClassName?: string;
    onChange?: ChangeEventHandler<HTMLTextAreaElement>;
};

export const Textarea = (props: TextareaProps) => {
    return (
        <div className="flex flex-column gap-2 mb-3">
            <label className={props.invalid ? 'p-error' : ''} htmlFor={props.id}>{props.label}</label>
            <InputTextarea
                className={props.className}
                id={props.id}
                value={props.value}
                onChange={props.onChange}
                readOnly={props.isPending || props.readOnly}
                rows={props.rows ?? 5}
                invalid={props.invalid}
            />
            {props.description && (<small>{props.description}</small>)}
            {props.errorMessage && (<small className="p-error">{props.errorMessage}</small>)}
        </div>
    );
};
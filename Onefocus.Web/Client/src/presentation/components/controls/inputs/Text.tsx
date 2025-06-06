import {InputText} from 'primereact/inputtext';
import {InputProps} from '../../../props/InputProps';
import {ChangeEventHandler} from 'react';

export type TextProps = InputProps & {
    floatClassName?: string;
    onChange?: ChangeEventHandler<HTMLInputElement>;
};

export const Text = (props: TextProps) => {
    return (
        <div className="flex flex-column gap-2 mb-3">
            <label htmlFor={props.id}>{props.label}</label>
            <InputText className={props.className} id={props.id} value={props.value} onChange={props.onChange}
                       readOnly={props.isPending || props.readOnly}/>
            {props.description && (<small>{props.description}</small>)}
        </div>
    );
};
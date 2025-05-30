import {InputTextarea } from 'primereact/inputtextarea';
import {InputProps} from '../../../props/InputProps';
import {FloatLabel} from '../panels';
import {ChangeEventHandler} from 'react';

export type TextareaProps = InputProps & {
    floatClassName?: string;
    onChange?: ChangeEventHandler<HTMLTextAreaElement>;
};

export const Textarea = (props: TextareaProps) => {
    return (
        <FloatLabel className={props.floatClassName}>
            <label htmlFor={props.id}>{props.label}</label>
            <InputTextarea className={props.className} id={props.id} value={props.value} onChange={props.onChange}
                       readOnly={props.isPending || props.readOnly}/>
        </FloatLabel>
    );
};
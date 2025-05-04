import {InputText} from 'primereact/inputtext';
import {InputProps} from '../../../props/InputProps';
import {FloatLabel} from '../panels';

export type TextProps = InputProps & {
    floatClassName?: string;
};

export const Text = (props: TextProps) => {
    return (
        <FloatLabel className={props.floatClassName}>
            <label htmlFor={props.id}>{props.label}</label>
            <InputText className={props.className} id={props.id} value={props.value} onChange={props.onChange}
                       readOnly={props.isPending || props.readOnly}/>
        </FloatLabel>
    );
};
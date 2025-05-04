import {Password as PiPassword} from 'primereact/password';
import {InputProps} from '../../../props/InputProps';
import {FloatLabel} from '../panels';

export type PasswordProps = InputProps & {
    floatClassName?: string;
    feedback?: boolean;
};

export const Password = (props: PasswordProps) => {
    return (
        <FloatLabel className={props.floatClassName}>
            <label htmlFor={props.id}>{props.label}</label>
            <PiPassword className={props.className} inputId={props.id} feedback={props.feedback ?? false}
                        value={props.value} onChange={props.onChange} readOnly={props.isPending || props.readOnly}/>
        </FloatLabel>
    );
};
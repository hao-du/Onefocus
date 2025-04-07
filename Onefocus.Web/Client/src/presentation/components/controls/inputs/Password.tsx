import {FloatLabel} from "primereact/floatlabel";
import {Password as PiPassword} from "primereact/password";
import {InputProps} from "../../../props/InputProps";

export type PasswordProps = InputProps & {
    floatClassName?: string;
    feedback?: boolean;
};

export const Password = (props: PasswordProps) => {
    return (
        <FloatLabel className={'mb-5 ' + (props.floatClassName ? props.floatClassName : '')}>
            <PiPassword className={props.className} inputId={props.id} feedback={props.feedback ?? false}
                        value={props.value} onChange={props.onChange} readOnly={props.isPending || props.readOnly}/>
            <label htmlFor={props.id}>{props.label}</label>
        </FloatLabel>
    );
};
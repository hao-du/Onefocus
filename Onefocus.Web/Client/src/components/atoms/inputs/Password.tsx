import {BaseProps} from "../../props/BaseProps.ts";
import {FloatLabel} from "primereact/floatlabel";
import { ChangeEventHandler } from "react";
import {Password as PiPassword} from "primereact/password";

type PasswordProps = BaseProps & {
    id: string;
    value: string;
    label: string;
    disabled?: boolean;
    onChange?: ChangeEventHandler<HTMLInputElement>;
    isPending?: boolean;
    floatClassName?: string;
    feedback?: boolean;
};

const Password = (props : PasswordProps) => {
    return (
        <FloatLabel className={'mb-5 ' + (props.floatClassName ? props.floatClassName : '')}>
            <PiPassword className={props.className} inputId={props.id} feedback={props.feedback ?? false} value={props.value} onChange={props.onChange} />
            <label htmlFor={props.id}>{props.label}</label>
        </FloatLabel>
    );
};

export default Password;
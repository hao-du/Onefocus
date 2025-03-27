import {BaseProps} from "../../props/BaseProps.ts";
import {InputText} from "primereact/inputtext";
import {FloatLabel} from "primereact/floatlabel";
import { ChangeEventHandler } from "react";

type ButtonProps = BaseProps & {
    id: string;
    value?: string | null | undefined;
    label: string;
    disabled?: boolean;
    onChange?: ChangeEventHandler<HTMLInputElement>;
    isPending?: boolean;
    floatClassName?: string;
};

const Text = (props : ButtonProps) => {
    return (
        <FloatLabel className={'mb-5 ' + (props.floatClassName ? props.floatClassName : '')}>
            <InputText className={props.className} id={props.id} value={props.value} onChange={props.onChange} />
            <label htmlFor={props.id}>{props.label}</label>
        </FloatLabel>
    );
};

export default Text;
import {InputText} from "primereact/inputtext";
import {FloatLabel} from "primereact/floatlabel";
import {InputProps} from "../../../props/InputProps";

export type TextProps = InputProps & {
    floatClassName?: string;
};

export const Text = (props: TextProps) => {
    return (
        <FloatLabel className={'mb-5 ' + (props.floatClassName ? props.floatClassName : '')}>
            <InputText className={props.className} id={props.id} value={props.value} onChange={props.onChange}
                       readOnly={props.isPending || props.readOnly}/>
            <label htmlFor={props.id}>{props.label}</label>
        </FloatLabel>
    );
};
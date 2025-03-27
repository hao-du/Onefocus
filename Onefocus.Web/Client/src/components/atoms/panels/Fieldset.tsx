import * as React from "react";
import {Fieldset as PiFieldset} from "primereact/fieldset";
import {BaseProps} from "../../props/BaseProps.ts";

type FieldsetProps = BaseProps & {
    children: React.ReactNode;
    title: string;
};

const Fieldset = (props: FieldsetProps) => {
    return (
        <PiFieldset legend={props.title} className={props.className}>
            {props.children}
        </PiFieldset>
    );
};

export default Fieldset;
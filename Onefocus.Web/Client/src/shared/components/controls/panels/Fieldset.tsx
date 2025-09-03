import * as React from 'react';
import { Fieldset as PrimeFieldset } from 'primereact/fieldset';
import { BaseProps } from '../../props';

type FieldsetProps = BaseProps & {
    children: React.ReactNode;
    title: string;
};

const Fieldset = (props: FieldsetProps) => {
    return (
        <PrimeFieldset legend={props.title} className={props.className}>
            {props.children}
        </PrimeFieldset>
    );
};

export default Fieldset;
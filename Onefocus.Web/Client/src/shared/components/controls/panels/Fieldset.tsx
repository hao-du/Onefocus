import * as React from 'react';
import { Fieldset as PrimeFieldset } from 'primereact/fieldset';
import { BaseProps } from '../../props';
import { useLocale } from '../../../hooks';

type FieldsetProps = BaseProps & {
    children: React.ReactNode;
    title: string;
};

const Fieldset = (props: FieldsetProps) => {
    const {translate} = useLocale();

    return (
        <PrimeFieldset legend={translate(props.title)} className={props.className}>
            {props.children}
        </PrimeFieldset>
    );
};

export default Fieldset;
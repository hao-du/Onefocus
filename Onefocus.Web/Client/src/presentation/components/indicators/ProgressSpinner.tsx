import {CSSProperties} from 'react';
import {ProgressSpinner as PiProgressSpinner} from 'primereact/progressspinner';
import {BaseProps} from '../../props/BaseProps';


type ProgressSpinnerProps = BaseProps & {
    style: CSSProperties;
}

export const ProgressSpinner = (props: ProgressSpinnerProps) => {
    return (
        <PiProgressSpinner
            style={props.style}
            strokeWidth="1"/>
    );
};
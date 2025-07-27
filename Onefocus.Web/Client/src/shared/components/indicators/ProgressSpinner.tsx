import {ProgressSpinner as PiProgressSpinner} from 'primereact/progressspinner';
import {BaseProps} from '../props/BaseProps';

type ProgressSpinnerProps = BaseProps & {
}

export const ProgressSpinner = (props: ProgressSpinnerProps) => {
    return (
        <PiProgressSpinner
            style={props.style}
            strokeWidth="1"/>
    );
};
import { ProgressSpinner as PiProgressSpinner } from 'primereact/progressspinner';
import { BaseProps } from '../props';

type ProgressSpinnerProps = BaseProps & {
}

export const ProgressSpinner = (props: ProgressSpinnerProps) => {
    return (
        <PiProgressSpinner
            style={props.style}
            strokeWidth="1"/>
    );
};
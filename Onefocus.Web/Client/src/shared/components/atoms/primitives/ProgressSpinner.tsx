import { ProgressSpinner as PiProgressSpinner } from 'primereact/progressspinner';
import { BaseIdentityProps, BaseStyleProps } from '../../../props/BaseProps';

interface ProgressSpinnerProps extends BaseIdentityProps, BaseStyleProps {
    strokeWidth?: number;
};

export const ProgressSpinner = (props: ProgressSpinnerProps) => {
    return (
        <PiProgressSpinner
            id={props.id}
            key={props.key}
            style={props.style}
            strokeWidth={props.strokeWidth?.toString() ?? "1"}
        />
    );
};
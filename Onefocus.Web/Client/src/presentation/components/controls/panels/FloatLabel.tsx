import {FloatLabel as PiFloatLabel, FloatLabelProps as PiFloatLabelProps} from 'primereact/floatlabel';
import {BaseProps} from '../../../props/BaseProps';

type FloatLabelProps = BaseProps & PiFloatLabelProps & {
};

export const FloatLabel = (props: FloatLabelProps) => {
    return (
        <PiFloatLabel className={'of-float-label mb-5 ' + (props.className ? props.className : '')}>
            {props.children}
        </PiFloatLabel>
    );
};
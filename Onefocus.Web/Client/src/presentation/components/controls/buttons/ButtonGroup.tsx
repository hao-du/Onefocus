import { ButtonGroup as PiButtonGroup } from 'primereact/buttongroup';
import {PropsWithChildren} from 'react';

type ButtonGroupProps = PropsWithChildren & {
};

export const ButtonGroup = (props: ButtonGroupProps) => {
    return (
        <PiButtonGroup>
            {props.children}
        </PiButtonGroup>
    );
};
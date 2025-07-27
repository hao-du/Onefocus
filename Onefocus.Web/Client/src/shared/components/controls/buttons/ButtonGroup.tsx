import { ButtonGroup as PiButtonGroup } from 'primereact/buttongroup';
import {PropsWithChildren} from 'react';

type ButtonGroupProps = PropsWithChildren & {
};

const ButtonGroup = (props: ButtonGroupProps) => {
    return (
        <PiButtonGroup>
            {props.children}
        </PiButtonGroup>
    );
};

export default ButtonGroup;
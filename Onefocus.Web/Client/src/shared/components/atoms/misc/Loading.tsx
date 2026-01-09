
import { PulseLoader } from 'react-spinners';
import type { ClassNameProps } from '../../../props/BaseProps';
import { SizeType } from '../../../types';
import { NUMBER_BY_SIZE_TYPE } from '../../../constants';

interface LoadingProps extends ClassNameProps {
    size?: SizeType;
}

export default function Loading(props: LoadingProps) {
    return (
        <PulseLoader
            size={NUMBER_BY_SIZE_TYPE[props.size ?? 'middle']}
            className={props.className}
        />
    );
}
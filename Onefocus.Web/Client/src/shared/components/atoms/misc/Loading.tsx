
import { ReactNode } from 'react';
import { PulseLoader } from 'react-spinners';
import type { IdentityProps } from '../../../props/BaseProps';
import { SizeType } from '../../../types';
import { NUMBER_BY_SIZE_TYPE } from '../../../constants';

interface LoadingProps extends IdentityProps {
    size?: SizeType;
    content?: ReactNode;
}

export default function Loading(props: LoadingProps) {
    return (
        <PulseLoader
            size={NUMBER_BY_SIZE_TYPE[props.size ?? 'middle']}
        >
            {props.content}
        </PulseLoader>
    );
}
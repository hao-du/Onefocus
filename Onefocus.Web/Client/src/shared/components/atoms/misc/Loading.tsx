
import { PulseLoader } from 'react-spinners';
import type { ClassNameProps } from '../../../props/BaseProps';
import { SizeType } from '../../../types';
import { NUMBER_BY_SIZE_TYPE } from '../../../constants';
import { joinClassNames } from '../../../utils';

interface LoadingProps extends ClassNameProps {
    size?: SizeType | 'xlarge';
}

const Loading = (props: LoadingProps) => {
    return (
        <PulseLoader
            size={NUMBER_BY_SIZE_TYPE[props.size ?? 'middle']}
            className={joinClassNames(props.className, 'flex whitespace-nowrap')}
        />
    );
}

export default Loading;
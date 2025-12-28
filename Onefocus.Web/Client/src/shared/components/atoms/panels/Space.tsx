import { Space as AntSpace } from 'antd';
import { ChildrenProps, ClassNameProps } from '../../../props/BaseProps';
import { SizeType } from 'antd/es/config-provider/SizeContext';

interface SpaceProps extends ClassNameProps, ChildrenProps {
    vertical: boolean;
    size?: SizeType | number;
}

const Space = ({
    vertical = false,
    size = 'middle',
    ...props
}: SpaceProps) => {
    return (
        <AntSpace
            vertical={vertical}
            size={size}
            className={props.className}
        >
            {props.children}
        </AntSpace>
    );
};

export default Space;
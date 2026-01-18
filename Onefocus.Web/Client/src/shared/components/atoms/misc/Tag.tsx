import { Tag as AntTag } from 'antd';
import type { ClassNameProps, IdentityProps } from '../../../props/BaseProps';
import { ColorType } from '../../../types';


interface EmptyProps extends IdentityProps, ClassNameProps {
    color?: ColorType;
    value?: string;
}

const Tag = (props: EmptyProps) => {
    return (
        <AntTag
            key={props.key}
            id={props.id}
            className={props.className}
            color={props.color}
            variant="outlined"
        >
            {props.value}
        </AntTag>
    );
}

export default Tag;
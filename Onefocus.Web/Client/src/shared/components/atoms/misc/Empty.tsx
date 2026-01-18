import { ReactNode } from 'react';
import { Empty as AntEmpty } from 'antd';
import type { ChildrenProps, ClassNameProps, LabelProps } from '../../../props/BaseProps';


interface EmptyProps extends LabelProps, ClassNameProps, ChildrenProps {
    image?: ReactNode
}

const Empty = (props: EmptyProps) => {
    return (
        <AntEmpty
            className={props.className}
            styles={{ image: { height: 'auto' } }}
            description={props.label}
            image={props.image}
        >
            {props.children}
        </AntEmpty>
    );
}

export default Empty;
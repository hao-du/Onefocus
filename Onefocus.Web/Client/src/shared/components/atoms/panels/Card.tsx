import { ReactNode } from 'react';
import { Card as AntCard } from 'antd';
import { ChildrenProps, ClassNameProps } from '../../../props/BaseProps';

interface CardProps extends ChildrenProps, ClassNameProps {
    title?: string;
    extra?: ReactNode
}

const Card = (props: CardProps) => {
    return (
        <AntCard
            title={props.title}
            className={props.className}
            extra={props.extra}
        >
            {props.children}
        </AntCard>
    );
};

export default Card;

import { useCallback, useMemo } from 'react';
import { Button as AntButton } from 'antd';
import type { BaseButtonProps } from '../../../props/BaseButtonProps';
import Icon from '../misc/Icon';
import { ButtonType } from '../../../types';

interface ButtonProps extends BaseButtonProps {
    text?: string;
    type?: ButtonType
    htmlType?: 'submit' | 'button' | 'reset';
}

export default function Button(props: ButtonProps) {
    const renderIcon = useMemo(() => {
        if (!props.icon) return undefined;
        if (!props.isPending) return props.icon;
        return <Icon name='user' />
    }, [props.icon, props.isPending]);

    const onInternalClick = useCallback((e: React.MouseEvent<HTMLElement, MouseEvent>) => {
        if (props.onClick) {
            e.preventDefault();
            props.onClick(e);
        }
    }, [props]);

    return (
        <AntButton
            id={props.id}
            key={props.key}
            type={props.type}
            className={props.className}
            styles={props.styles}
            disabled={props.disabled || props.isPending}
            icon={renderIcon}
            onClick={onInternalClick}
            htmlType={props.htmlType ?? 'button'}
            {...props}
        >
            {props.text}
        </AntButton>
    );
}
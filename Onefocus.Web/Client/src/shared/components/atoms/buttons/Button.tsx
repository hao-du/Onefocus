
import { useCallback } from 'react';
import { Button as AntButton } from 'antd';
import type { BaseButtonProps } from '../../../props/BaseButtonProps';
import { ButtonType } from '../../../types';
import Loading from '../misc/Loading';

interface ButtonProps extends BaseButtonProps {
    text?: string;
    type?: ButtonType;
    htmlType?: 'submit' | 'button' | 'reset';
    block?: boolean;
}

export default function Button(props: ButtonProps) {
    const onInternalClick = useCallback((e: React.MouseEvent<HTMLElement, MouseEvent>) => {
        if (props.onClick) {
            e.preventDefault();
            props.onClick(e);
        }
    }, [props]);

    return (
        <AntButton
            {...props}
            id={props.id}
            key={props.key}
            type={props.type ?? 'primary'}
            className={props.className}
            styles={props.styles}
            disabled={props.disabled || props.isPending}
            icon={!props.icon || !props.isPending ? props.icon : <Loading size="small" />}
            onClick={onInternalClick}
            htmlType={props.htmlType ?? 'button'}
            block={props.block}

        >
            {props.isPending && !props.icon ? <Loading /> : props.text}
        </AntButton>
    );
}
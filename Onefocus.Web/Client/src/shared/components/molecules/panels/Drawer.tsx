import { useCallback, useState } from 'react';
import { Drawer as AntDrawer } from 'antd';
import { ChildrenProps, ClassNameProps } from '../../../props/BaseProps';
import { ActionOption } from '../../../options/ActionOption';
import DropdownButton from '../buttons/DropdownButton';
import Icon from '../../atoms/misc/Icon';

interface DrawerProps extends ClassNameProps, ChildrenProps {
    title?: string;
    open?: boolean;
    showPrimaryButton?: boolean;
    actions?: ActionOption[];
    onClose?: () => void;
}

const Drawer = (props: DrawerProps) => {
    const [collapsed, setCollapsed] = useState(false);

    const onToggle = useCallback(() => {
        setCollapsed(prev => !prev);
    }, []);

    return (
        <AntDrawer
            title={props.title}
            size={collapsed ? 10 : undefined}
            closable={{ placement: 'end' }}
            open={props.open}
            onClose={props.onClose}
            mask={{ enabled: false }}
            extra={
                (props.actions ?? []).length > 0 && (
                    <DropdownButton
                        showPrimaryButton={props.showPrimaryButton}
                        actions={props.actions}
                    />
                )
            }
        >
            {props.children}
            {props.open && (
                <div className="absolute top-16 -left-3 w-3 h-20 border-t bg-(--ant-color-bg-container) rounded-l-md border-(--ant-color-border-secondary) flex flex-col justify-center cursor-pointer" onClick={onToggle}>
                    <div className="w-3 h-3">
                        <Icon name={collapsed ? 'chevronLeft' : 'chevronRight'} size='small' />
                    </div>
                </div>
            )}
        </AntDrawer>
    );
};

export default Drawer;
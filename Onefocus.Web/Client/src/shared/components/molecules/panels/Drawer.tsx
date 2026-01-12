import { Drawer as AntDrawer } from 'antd';
import { ChildrenProps, ClassNameProps } from '../../../props/BaseProps';
import { ActionOption } from '../../../options/ActionOption';
import DropdownButton from '../buttons/DropdownButton';

interface DrawerProps extends ClassNameProps, ChildrenProps {
    title?: string;
    open?: boolean;
    showPrimaryButton?: boolean;
    actions?: ActionOption[];
    onClose?: () => void;
}

const Drawer = (props: DrawerProps) => {
    return (
        <AntDrawer
            title={props.title}
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
        </AntDrawer>
    );
};

export default Drawer;
import { ChildrenProps } from '../../props/BaseProps';
import { ActionOption } from '../../options/ActionOption';
import DefaultTemplate from '../templates/DefaultTemplate';
import { MenuItems } from '../../datasources';

interface DefaultLayoutProps extends ChildrenProps {
    title?: string;
    actions?: ActionOption[];
    showPrimaryButton?: boolean;
    workspaceClassName?: string;
};

const DefaultLayout = (props: DefaultLayoutProps) => {
    return (
        <DefaultTemplate
            title={props.title}
            menuOptions={MenuItems}
            showPrimaryButton
            workSpaceActions={props.actions}
            workspaceClassName={props.workspaceClassName}
        >
            {props.children}
        </DefaultTemplate>
    );
};

export default DefaultLayout;
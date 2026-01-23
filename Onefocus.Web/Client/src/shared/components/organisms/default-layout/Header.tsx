/* eslint-disable @typescript-eslint/no-empty-object-type */
import { ChildrenProps } from '../../../props/BaseProps';

interface HeaderProps extends ChildrenProps {
};

function Header(props: HeaderProps) {
    return (
        <div className="p-3 bg-(--ant-color-bg-container) border-b border-(--ant-color-border-secondary) h-15 shrink-0 shadow-md z-1">
            {props.children}
        </div>
    );
};

export default Header;
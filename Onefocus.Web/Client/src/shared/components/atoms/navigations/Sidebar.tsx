import { Sidebar as PrimeSideBar } from "primereact/sidebar";
import { BaseHtmlProps, BaseIdentityProps, ChildrenProps } from "../../../props/BaseProps";

interface SidebarProps extends ChildrenProps, BaseIdentityProps, BaseHtmlProps {
    visible?: boolean;
    onHide(): void;
};

export const Sidebar = (props: SidebarProps) => {
    return (
        <PrimeSideBar 
            visible={props.visible} 
            onHide={props.onHide} 
            className={props.className}
        >
            {props.children}
        </PrimeSideBar>
    );
};
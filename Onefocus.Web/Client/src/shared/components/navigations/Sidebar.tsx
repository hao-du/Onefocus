import { Sidebar as PrimeSideBar } from "primereact/sidebar";
import * as React from "react";

type SidebarProps = {
    visible?: boolean;
    children: React.ReactNode;
    onHide(): void;
};

const Sidebar = (props: SidebarProps) => {
    return (
        <PrimeSideBar visible={props.visible} onHide={props.onHide} className="md:hidden">
            {props.children}
        </PrimeSideBar>
    );
}

export default Sidebar;
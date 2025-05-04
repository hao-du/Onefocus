import {Sidebar as PiSideBar} from "primereact/sidebar";
import * as React from "react";

type SidebarProps = {
    visible?: boolean;
    children: React.ReactNode;
    onHide(): void;
};

export const Sidebar = (props: SidebarProps) => {
    return (
        <PiSideBar visible={props.visible} onHide={props.onHide} className="md:hidden">
            {props.children}
        </PiSideBar>
    );
}

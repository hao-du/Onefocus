import { PanelMenu } from 'primereact/panelmenu';
import { useState} from "react";
import {SideMenuItem, MobileSidebarVisibleState} from "./SideMenu.interface";
import {Sidebar} from "primereact/sidebar";

type SideMenuProps = {
    items: SideMenuItem[];
    mobileVisibleState: MobileSidebarVisibleState;
};

export const SideMenu = (props: SideMenuProps) => {
    const [expandedKeys, setExpandedKeys] = useState<SideMenuItem>({key: -1});

    const renderSideMenu = () => {
        return (
            <>
                <h1 className="mt-0 mb-1 text-6xl font-normal text-primary">Onefocus</h1>
                <PanelMenu model={props.items} expandedKeys={expandedKeys} onExpandedKeysChange={setExpandedKeys} className="w-full" multiple />
            </>

        );
    };

    return (
        <>
            {/* Desktop Sidebar */}
            <div className="hidden md:block of-sidemenu w-18rem bg-gray-100 p-3">
                {renderSideMenu()}
            </div>

            {/* Mobile Sidebar */}
            <Sidebar visible={props.mobileVisibleState.mobileSidebarVisible} onHide={() => props.mobileVisibleState.setMobileSidebarVisible(false)} className="md:hidden">
                {renderSideMenu()}
            </Sidebar>
        </>
    );
}

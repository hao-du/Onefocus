import {PanelMenu} from 'primereact/panelmenu';
import {useCallback, useMemo, useState} from "react";
import {MobileSidebarVisibleState, SideMenuItem} from "./SideMenu.interface";
import {Sidebar} from "primereact/sidebar";

type SideMenuProps = {
    items: SideMenuItem[];
    mobileVisibleState: MobileSidebarVisibleState;
};

export const SideMenu = (props: SideMenuProps) => {
    const expandAllItems = useMemo((): SideMenuItem[] => {
        return props?.items?.map(item => {
            if(!item.items || item.items?.length == 0) {
                return item;
            }

            return {
                ...item,
                expanded: true,
                items: item.items
            };
        });
    }, [props?.items]);

    const [expandedKeys, setExpandedKeys] = useState<SideMenuItem[]>(expandAllItems);

    const renderSideMenu = useCallback(() => {
        return (
            <>
                <h1 className="mt-0 mb-1 text-6xl font-normal text-primary">Onefocus</h1>
                <PanelMenu model={props.items} expandedKeys={expandedKeys} onExpandedKeysChange={setExpandedKeys}
                           className="w-full" multiple/>
            </>

        );
    }, [props.items, expandedKeys]);

    return (
        <>
            {/* Desktop Sidebar */}
            <div className="hidden md:block of-sidemenu w-18rem bg-gray-100 p-3">
                {renderSideMenu()}
            </div>

            {/* Mobile Sidebar */}
            <Sidebar visible={props.mobileVisibleState.mobileSidebarVisible}
                     onHide={() => props.mobileVisibleState.setMobileSidebarVisible(false)} className="md:hidden">
                {renderSideMenu()}
            </Sidebar>
        </>
    );
}

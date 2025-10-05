import { PanelMenu } from 'primereact/panelmenu';
import { Sidebar } from "primereact/sidebar";
import { useCallback, useMemo, useState } from "react";
import MobileSidebarVisibleState from "./interfaces/MobileSidebarVisibleState";
import SideMenuItem from "./interfaces/SideMenuItem";
import { useTranslatedMenuItems } from './hooks/useTranslatedMenu';
import { APP_NAME } from '../../hooks/constants/Locale';

type SideMenuProps = {
    items?: SideMenuItem[];
    mobileVisibleState: MobileSidebarVisibleState;
};

const SideMenu = (props: SideMenuProps) => {
    const translatedItems = useTranslatedMenuItems(props.items);

    const expandAllItems = useMemo((): SideMenuItem[] => {
        if (!props?.items) return [];
        
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
                <h1 className="mt-0 mb-1 text-6xl font-normal text-primary">{APP_NAME}</h1>
                <PanelMenu model={translatedItems} expandedKeys={expandedKeys} onExpandedKeysChange={setExpandedKeys}
                           className="w-full flex-auto overflow-auto" multiple/>
            </>

        );
    }, [translatedItems, expandedKeys]);

    return (
        <>
            {/* Desktop Sidebar */}
            <div className="hidden md:flex flex-column of-sidemenu w-18rem bg-gray-100 p-3">
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

export default SideMenu;
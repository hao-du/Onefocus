import {MenuItem} from "primereact/menuitem";

export interface SideMenuItem extends MenuItem {
    key: number
};

export interface MobileSidebarVisibleState {
    mobileSidebarVisible: boolean;
    setMobileSidebarVisible: React.Dispatch<React.SetStateAction<boolean>>;
}
import {useState,useRef} from "react";
import {BaseProps} from "../props/BaseProps";
import {Sidebar} from "primereact/sidebar";
import {Button} from "primereact/button";
import {Avatar} from "primereact/avatar";
import {OverlayPanel} from "primereact/overlaypanel";


type AppLayoutProps = BaseProps & {
    children: React.ReactNode;
};

const SideMenu = () => {
    return (
        <>
            <div className="text-xl font-bold mb-3">Menu</div>
            <ul className="list-none p-0 m-0">
                <li className="mb-2 cursor-pointer">Dashboard</li>
                <li className="mb-2 cursor-pointer">Settings</li>
                <li className="mb-2 cursor-pointer">Reports</li>
            </ul>
        </>
    )
}

const AppLayout = (props: AppLayoutProps) => {
    const [mobileSidebarVisible, setMobileSidebarVisible] = useState(false);
    const profilePanelRef = useRef<OverlayPanel>(null);

    const handleProfileClick = (event: React.MouseEvent<HTMLSpanElement, MouseEvent>) => {
        if(profilePanelRef.current)
            profilePanelRef.current.toggle(event);
    };

    return (
        <div className="flex h-full">
            {/* Desktop Sidebar */}
            <div className="hidden md:block w-18rem bg-gray-100 p-3">
                <SideMenu />
            </div>

            {/* Mobile Sidebar */}
            <Sidebar visible={mobileSidebarVisible} onHide={() => setMobileSidebarVisible(false)} className="md:hidden">
                <SideMenu />
            </Sidebar>

            {/* Main Content Area */}
            <div className="flex flex-column flex-1 h-screen">
                {/* Header */}
                <div className="flex justify-content-between align-items-center border-bottom-1 surface-border px-3 py-2">
                    <div className="flex align-items-center gap-2">
                        {/* Hamburger menu for mobile */}
                        <Button
                            text
                            icon="pi pi-bars"
                            className="md:hidden"
                            onClick={() => setMobileSidebarVisible(true)}
                        />

                        {/* Action buttons (hidden on small screens if needed) */}
                        <div className="hidden md:flex gap-2">
                            <Button label="Save" icon="pi pi-save" />
                            <Button label="Edit" icon="pi pi-pencil" className="p-button-secondary" />
                            <Button label="Cancel" icon="pi pi-times" className="p-button-danger" />
                        </div>
                    </div>

                    {/* Profile avatar + panel */}
                    <div>
                        <Avatar icon="pi pi-user" shape="circle" className="cursor-pointer" onClick={handleProfileClick} />
                        <OverlayPanel ref={profilePanelRef}>
                            <div className="p-2">
                                <div className="font-bold mb-2">John Doe</div>
                                <Button label="Logout" icon="pi pi-sign-out" className="p-button-text p-0" />
                            </div>
                        </OverlayPanel>
                    </div>
                </div>

                {/* Main Workspace */}
                <div className="flex-1 overflow-auto p-3 bg-gray-50">
                    <h1 className="text-2xl font-bold mb-3">Main Workspace</h1>
                    {props.children}
                </div>
            </div>
        </div>
    );
};

export default AppLayout;


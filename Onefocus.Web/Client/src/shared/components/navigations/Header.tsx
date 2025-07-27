import {Button} from "primereact/button";
import {Avatar} from "primereact/avatar";
import {OverlayPanel} from "primereact/overlaypanel";
import {useRef} from "react";
import MobileSidebarVisibleState from "./interfaces/MobileSidebarVisibleState";
import { useWindows } from "../hooks";

type HeaderProps = {
    mobileVisibleState: MobileSidebarVisibleState;
};

const Header = (props: HeaderProps) => {
    const profilePanelRef = useRef<OverlayPanel>(null);
    const {isMobile} = useWindows();

    const handleProfileClick = (event: React.MouseEvent<HTMLSpanElement, MouseEvent>) => {
        if (profilePanelRef.current)
            profilePanelRef.current.toggle(event);
    };

    return (
        <div className="flex justify-content-between align-items-center surface-background-color px-3 py-3">
            <div className="flex align-items-center gap-2">
                {isMobile && (
                    <Button
                        text
                        icon="pi pi-bars"
                        className="text-primary-900"
                        onClick={() => props.mobileVisibleState.setMobileSidebarVisible(true)}
                    />
                )}

                {/* Action buttons (hidden on small screens if needed)
                <div className="hidden md:flex gap-2">
                    <Button label="Save" icon="pi pi-save" />
                    <Button label="Edit" icon="pi pi-pencil" className="p-button-secondary" />
                    <Button label="Cancel" icon="pi pi-times" className="p-button-danger" />
                </div>
                */}
            </div>

            {/* Profile avatar + panel */}
            <div>
                <Avatar icon="pi pi-user" size="large" shape="circle" className="cursor-pointer"
                        onClick={handleProfileClick}/>
                <OverlayPanel ref={profilePanelRef}>
                    <div className="p-2">
                        <div className="font-bold mb-2">John Doe</div>
                        <Button label="Logout" icon="pi pi-sign-out" className="p-button-text p-0"/>
                    </div>
                </OverlayPanel>
            </div>
        </div>
    );
}

export default Header;
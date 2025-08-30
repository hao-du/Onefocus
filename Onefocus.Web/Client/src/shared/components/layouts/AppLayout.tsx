import { useState } from 'react';
import { useWindows } from '../hooks';
import { Header, SideMenu, SideMenuItem } from '../navigations';
import { BaseProps } from '../props';

type AppLayoutProps = BaseProps & {
    children: React.ReactNode;
    items?: SideMenuItem[];
};

const AppLayout = (props: AppLayoutProps) => {
    const [mobileSidebarVisible, setMobileSidebarVisible] = useState(false);
    const {isMobile} = useWindows();

    return (
        <div className={isMobile ? undefined : "flex h-full"}>
            <SideMenu items={props.items} mobileVisibleState={{ mobileSidebarVisible, setMobileSidebarVisible }} />

            {/* Main Content Area */}
            <div className="flex flex-column flex-1 h-screen">
                <Header mobileVisibleState={{ mobileSidebarVisible, setMobileSidebarVisible }} />

                {/* Main Workspace */}
                <div className="flex-1 overflow-auto bg-gray-50">
                    {props.children}
                </div>
            </div>
        </div>
    );
};

export default AppLayout;
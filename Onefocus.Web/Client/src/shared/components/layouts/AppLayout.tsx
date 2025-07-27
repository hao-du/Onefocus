import { useState } from 'react';
import { BaseProps } from '../props';
import { Header, SideMenu, SideMenuItem } from '../navigations';

type AppLayoutProps = BaseProps & {
    children: React.ReactNode;
    items?: SideMenuItem[];
};

const AppLayout = (props: AppLayoutProps) => {
    const [mobileSidebarVisible, setMobileSidebarVisible] = useState(false);

    return (
        <div className="flex h-full">
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
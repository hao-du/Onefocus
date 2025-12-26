import { useState } from 'react';
import { useWindows } from '../hooks';
import { Header, SideMenu, SideMenuItem } from '../navigations';
import { BaseProps } from '../props';
import { ActionItem } from '../controls';

type AppLayoutProps = BaseProps & {
    children: React.ReactNode;
    items?: SideMenuItem[];
    profileActions?: ActionItem[]
};

const AppLayout = (props: AppLayoutProps) => {
    const [mobileSidebarVisible, setMobileSidebarVisible] = useState(false);
    const { isMobile } = useWindows();

    return (
        <div className={isMobile ? "h-screen" : "flex h-screen"}>
            <SideMenu items={props.items} mobileVisibleState={{ mobileSidebarVisible, setMobileSidebarVisible }} />

            {/* Main Content Area */}
            <div className="flex flex-column flex-1 h-full">
                <Header mobileVisibleState={{ mobileSidebarVisible, setMobileSidebarVisible }} actions={props.profileActions}/>

                {/* Main Workspace */}
                <div className="flex-auto overflow-auto bg-gray-50">
                    <div className="h-full w-full">
                        {props.children}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AppLayout;
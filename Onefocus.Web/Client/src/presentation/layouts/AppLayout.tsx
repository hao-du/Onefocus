import {useState} from 'react';
import {useNavigate} from 'react-router';
import {BaseProps} from '../props/BaseProps';
import {Header, SideMenu, SideMenuItem} from '../components/navigations';


type AppLayoutProps = BaseProps & {
    children: React.ReactNode;
};

export const AppLayout = (props: AppLayoutProps) => {
    const navigate = useNavigate();

    const items: SideMenuItem[] = [
        {
            key: 0,
            label: 'Home',
            icon: 'pi pi-home',
            command: () => {
                navigate("/");
            }
        },
        {
            key: 1,
            label: 'Wallet',
            icon: 'pi pi-wallet',
            items: [
                {
                    label: 'Dashboard',
                    icon: 'pi pi-gauge',
                    command: () => {
                        navigate("/wallet");
                    }
                },
                {
                    label: 'Your traces',
                    icon: 'pi pi-calculator',
                    command: () => {
                        navigate("/wallet/traces");
                    }
                },
                {
                    label: 'Currency',
                    icon: 'pi pi-dollar',
                    command: () => {
                        navigate("/wallet/currency");
                    }
                },
                {
                    label: 'Bank',
                    icon: 'pi pi-credit-card',
                    command: () => {
                        navigate("/wallet/bank");
                    }
                }
            ]
        },
        {
            key: 2,
            label: 'Admin',
            icon: 'pi pi-user',
            items: [
                {
                    label: 'Users',
                    icon: 'pi pi-users',
                    command: () => {
                        navigate("/admin/users");
                    }
                },
                {
                    label: 'Settings',
                    icon: 'pi pi-cog',
                    command: () => {
                        navigate("/admin/settings");
                    }
                }
            ]
        }
    ];

    const [mobileSidebarVisible, setMobileSidebarVisible] = useState(false);

    return (
        <div className="flex h-full">
            <SideMenu items={items} mobileVisibleState={{mobileSidebarVisible, setMobileSidebarVisible}}/>

            {/* Main Content Area */}
            <div className="flex flex-column flex-1 h-screen">
                <Header mobileVisibleState={{mobileSidebarVisible, setMobileSidebarVisible}}/>

                {/* Main Workspace */}
                <div className="flex-1 overflow-auto bg-gray-50">
                    {props.children}
                </div>
            </div>
        </div>
    );
};


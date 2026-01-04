import { useState } from 'react';
import { MenuOption } from '../../../options/MenuOption';
import Section from '../../atoms/panels/Section';
import Menu from '../../molecules/navigations/menu/Menu';
import Button from '../../atoms/buttons/Button';
import Icon from '../../atoms/misc/Icon';
import AppTitle from '../../atoms/typography/AppTitle';

interface SidebarProps {
    menuItems: MenuOption[];
};

const Sidebar = (props: SidebarProps) => {
    const [sidebarOpen, setSidebarOpen] = useState(false);

    return (
        <>
            {sidebarOpen && (
                <div
                    className="fixed inset-0 bg-(--ant-color-bg-mask) z-1 lg:hidden"
                    onClick={() => setSidebarOpen(false)}
                />
            )}
            <div
                className={`
                    h-full
                    border-r border-(--ant-color-border-secondary)
                    left-0 z-1 w-3xs bg-(--ant-color-bg-container)
                    transition-transform duration-300 ease-in-out
                    fixed lg:relative lg:translate-x-0
                    ${sidebarOpen ? 'translate-x-0' : '-translate-x-full lg:translate-x-0'}
                `}
            >
                <div className="overflow-y-auto h-full">
                    <Section className="px-4 py-1 h-15 content-center sticky top-0 z-1 bg-(--ant-color-bg-container)">
                        <AppTitle title='Onefocus' />
                    </Section>
                    <Menu className="border-r-0" items={props.menuItems} expandAll={true} />
                </div>
            </div>

            <Button
                type="text"
                onClick={() => setSidebarOpen(true)}
                className="lg:hidden! fixed! inset-4"
                icon={<Icon name="hambugger" />}
            />
        </>
    );
};

export default Sidebar;
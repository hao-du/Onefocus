import React from 'react';
import { MenuOption } from '../../options/MenuOption';
import Sidebar from '../organisms/default-layout/Sidebar';
import Header from '../organisms/default-layout/Header';
import Workspace from '../organisms/default-layout/Workspace';
import { ChildrenProps } from '../../props/BaseProps';
import { ActionOption } from '../../options/ActionOption';

interface DefaultLayoutProps extends ChildrenProps {
    header?: React.ReactNode;
    title?: string;
    menuOptions?: MenuOption[];
    workSpaceActions?: ActionOption[];
    showPrimaryButton?: boolean;
};

export default function DefaultLayout(props: DefaultLayoutProps) {
    return (
        <div className="flex w-screen h-screen">
            <Sidebar menuItems={props.menuOptions ?? []}></Sidebar>

            <div className="flex-1 flex flex-col">
                <Header>{props.header}</Header>
                <Workspace title={props.title} actions={props.workSpaceActions} showPrimaryAction={props.showPrimaryButton}>{props.children}</Workspace>
            </div>
        </div>
    );
};
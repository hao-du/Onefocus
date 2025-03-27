import * as React from "react";
import {BaseProps} from "../props/BaseProps.ts";

type DefaultLayoutProps = BaseProps & {
    children: React.ReactNode;
    alignCenter?: boolean;
    justifyContentCenter?: boolean;
};

const DefaultLayout = (props: DefaultLayoutProps) => {
    const topDivClass = 'h-screen flex' + (props.alignCenter ? ' align-items-center' : '') + (props.alignCenter ? ' justify-content-center' : '');
    const secondDivClass = 'flex flex-column' + (props.alignCenter ? ' align-items-center' : '');

    return (
        <div className={topDivClass}>
            <div className={secondDivClass}>
                {props.children}
            </div>
        </div>
    );
};

export default DefaultLayout;


import * as React from 'react';
import {BaseProps} from '../props/BaseProps';

type SingleContentLayoutProps = BaseProps & {
    children: React.ReactNode;
    alignCenter?: boolean;
    justifyContentCenter?: boolean;
};

export const SingleContentLayout = (props: SingleContentLayoutProps) => {
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


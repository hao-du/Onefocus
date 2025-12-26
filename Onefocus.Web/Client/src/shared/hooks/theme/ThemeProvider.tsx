/* eslint-disable @typescript-eslint/no-empty-object-type */

import { ConfigProvider, App } from 'antd';
import { autoPrefixTransformer, StyleProvider } from '@ant-design/cssinjs';
import ThemeContext from './ThemeContext';
import { cssClassDefinitions } from './CssClassDefinitions';
import type { ChildrenProps } from '../../props/BaseProps';
import { useMemo } from 'react';

interface ThemeProviderProps extends ChildrenProps {
}

export default function ThemeProvider(props: ThemeProviderProps) {
    const value = useMemo(() => ({
        cssClasses: cssClassDefinitions
    }), []);

    return (
        <StyleProvider transformers={[autoPrefixTransformer]}>
            <ConfigProvider>
                <App>
                    <ThemeContext.Provider value={value}>
                        {props.children}
                    </ThemeContext.Provider>
                </App>
            </ConfigProvider>
        </StyleProvider>
    );
}

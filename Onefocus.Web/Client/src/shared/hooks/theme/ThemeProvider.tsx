/* eslint-disable @typescript-eslint/no-empty-object-type */

import { ConfigProvider, App, theme } from 'antd';
import { autoPrefixTransformer, StyleProvider } from '@ant-design/cssinjs';
import ThemeContext from './ThemeContext';
import { cssClassDefinitions } from './CssClassDefinitions';
import type { ChildrenProps } from '../../props/BaseProps';
import { useMemo } from 'react';
import ThemeContextValue from './ThemeContextValue';

interface ThemeProviderProps extends ChildrenProps {
}

const ThemeProvider = (props: ThemeProviderProps) => {
    const { token } = theme.useToken();

    const value = useMemo<ThemeContextValue>(() => ({
        cssClasses: cssClassDefinitions,
        styles: {
            size: {
                base: token.sizeUnit,
                margin: token.margin,
                marginSM: token.marginSM,
                marginMD: token.marginMD,
                marginLG: token.marginLG,
                marginXL: token.marginXL,
                marginXXL: token.marginXXL
            }
        }
    }), [token]);

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
};

export default ThemeProvider;

/* eslint-disable @typescript-eslint/no-empty-object-type */

import { ConfigProvider, App, theme } from 'antd';
import { autoPrefixTransformer, StyleProvider } from '@ant-design/cssinjs';
import ThemeContext from './ThemeContext';
import { cssClassDefinitions } from './CssClassDefinitions';
import type { ChildrenProps } from '../../props/BaseProps';
import { useMemo } from 'react';
import ThemeContextValue from './ThemeContextValue';

const { defaultAlgorithm } = theme;

interface ThemeProviderProps extends ChildrenProps {
}

const ThemeInner = ({ children }: ChildrenProps) => {
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
                marginXXL: token.marginXXL,
                padding: token.sizeSM,
            },
        },
    }), [token]);

    return (
        <ThemeContext.Provider value={value}>
            {children}
        </ThemeContext.Provider>
    );
};

const ThemeProvider = (props: ThemeProviderProps) => {
    return (
        <StyleProvider transformers={[autoPrefixTransformer]}>
            <ConfigProvider theme={{
                algorithm: defaultAlgorithm,
                components: {
                    Menu: {
                        lineWidth: 0,
                        itemPaddingInline: 0,
                    }
                }
            }}>
                <App>
                    <ThemeInner>
                        {props.children}
                    </ThemeInner>
                </App>
            </ConfigProvider>
        </StyleProvider>
    );
};

export default ThemeProvider;

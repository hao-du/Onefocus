import { forwardRef, useImperativeHandle, useRef } from 'react';
import { Menu as PiMenu } from 'primereact/menu';

import { ActionItem } from '../../options/ActionItem';
import { useLocale } from '../../../../hooks/locale/LocaleContext';
import { BaseHtmlProps, BaseIdentityProps } from '../../../../props/BaseProps';

interface MenuProps extends BaseIdentityProps, BaseHtmlProps {
    model?: ActionItem[];
    popupAlignment?: 'left' | 'right';
};

export type MenuRef = {
    toggle: (event: React.MouseEvent<HTMLElement>) => void;
};

export const Menu = forwardRef<MenuRef, MenuProps>((props, ref) => {
    const { translate } = useLocale();
    const internalRef = useRef<PiMenu>(null);

    useImperativeHandle(ref, () => ({
        toggle: (event: React.MouseEvent<HTMLElement>) => {
            internalRef.current?.toggle(event);
        }
    }));

    return (
        <PiMenu
            id={props.id}
            key={props.key}
            model={props.model?.map(item => {
                item.label = translate(item.label);
                return item;
            })}
            className={props.className}
            popupAlignment={props.popupAlignment ?? 'right'}
            
            ref={internalRef}
            popup
        />
    );
});
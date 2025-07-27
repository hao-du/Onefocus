import { Menu as PiMenu } from 'primereact/menu';
import { ActionItem } from '../interfaces';
import { BaseProps } from '../../props';
import { forwardRef, useImperativeHandle, useRef } from 'react';

type MenuProps = BaseProps & {
    model?: ActionItem[];
    popupAlignment?: 'left' | 'right';
    id?: string;
};

export type MenuRef = {
    toggle: (event: React.MouseEvent<HTMLElement>) => void;
};

const Menu = forwardRef<MenuRef, MenuProps>((props, ref) => {
    const internalRef = useRef<PiMenu>(null);

    useImperativeHandle(ref, () => ({
        toggle: (event: React.MouseEvent<HTMLElement>) => {
            internalRef.current?.toggle(event);
        }
    }));

    return (
        <PiMenu
            model={props.model}
            popupAlignment={props.popupAlignment ?? 'right'}
            id={props.id}
            ref={internalRef}
            popup
        />
    );
});

export default Menu;
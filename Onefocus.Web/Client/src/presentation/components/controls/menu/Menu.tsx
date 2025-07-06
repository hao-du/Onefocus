import {Menu as PiMenu} from 'primereact/menu';
import {ActionItem} from '../interfaces/ActionItem';

type MenuProps = BaseProps & {
    model: ActionItem[];
    popupAlignment?: 'left' | 'right';
    id?: string;
    ref?: string;
};

export const Menu = (props: MenuProps) => {
    return (
        <PiMenu
            model={props.model}
            popupAlignment={props.popupAlignment ?? 'right'}
            id={props.id}
            ref={props.ref}
            popup
        />
    );
};
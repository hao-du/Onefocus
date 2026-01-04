
import { Dropdown as AntDropdown } from 'antd';
import type { MenuItemType } from 'antd/es/menu/interface';

import { ChildrenProps } from '../../../props/BaseProps';
import { ActionOption } from '../../../options/ActionOption';

import { useMemo } from 'react';
import { getGuid } from '../../../utils';

interface DropdownProps extends ChildrenProps {
    actions?: ActionOption[];
    placement?: 'bottomRight';
}

const Dropdown = (props: DropdownProps) => {
    const menuItems: MenuItemType[] = useMemo(() => {
        if (!props.actions?.length) return [];

        return props.actions.map((action): MenuItemType => ({
            key: action.id ?? action.key ?? getGuid(),
            label: action.label,
            icon: action.icon,
            disabled: action.disabled || action.isPending,
            onClick: action.command,
            type: 'item',
        }));
    }, [props.actions]);

    return (
        <AntDropdown
            placement={props.placement}
            menu={{ items: menuItems }}
            trigger={['click']}
        >
            {props.children}
        </AntDropdown>
    );
}

export default Dropdown;
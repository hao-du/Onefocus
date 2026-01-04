import { MenuProps } from "antd";
import { MenuOption } from "../../../../options/MenuOption";
import { NavigableOption } from "../../../../options/NavigableOption";
import { getGuid } from "../../../../utils";

type AntMenuItem = Required<MenuProps>['items'][number];
export const toAntMenuItems = (items: MenuOption[]): AntMenuItem[] => {
    return items.map((item): AntMenuItem => {
        // Divider
        if (item.type === 'divider') {
            return {
                type: 'divider',
            };
        }

        // Group
        if (item.type === 'group') {
            return {
                type: 'group',
                label: item.label,
                children: toAntMenuItems(item.children ?? []),
            };
        }

        // SubMenu
        if (item.children?.length) {
            return {
                key: item.key ?? getGuid(),
                icon: item.icon,
                label: item.label,
                children: toAntMenuItems(item.children),
            };
        }

        // Normal item
        return {
            key: item.key ?? getGuid(),
            icon: item.icon,
            label: item.label,
        };
    });
};

export const menuItemsToFlattenMap = (items: MenuOption[], map = new Map<string, NavigableOption>()): Map<string, NavigableOption> => {
    for (const item of items) {
        if (item.url) {
            map.set(item.key ?? getGuid(), {
                key: item.key,
                label: item.label,
                url: item.url,
            });
        }

        if (item.children) {
            menuItemsToFlattenMap(item.children, map);
        }
    }

    return map;
};

export const getAllMenuOptionKeys = (items: MenuOption[]): string[] => {
    const keys: string[] = [];

    const traverse = (list: MenuOption[]) => {
        for (const item of list) {
            if (item.children?.length) {
                keys.push(item.key ?? getGuid());
                traverse(item.children);
            }
        }
    };

    traverse(items);
    return keys;
};
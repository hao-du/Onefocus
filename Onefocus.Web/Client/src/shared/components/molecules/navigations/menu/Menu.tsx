import { useCallback, useMemo } from "react";
import { Menu as AntMenu } from "antd";
import { useNavigate } from "react-router";
import { ClassNameProps } from "../../../../props/BaseProps";
import { MenuOption } from "../../../../options/MenuOption";
import { NavigableOption } from "../../../../options/NavigableOption";
import { getAllMenuOptionKeys, menuItemsToFlattenMap, toAntMenuItems } from "./menuUtils";
import { getRemUnit } from "../../../../utils";

interface MenuProps extends ClassNameProps {
    items: MenuOption[];
    onItemClick?: (item: NavigableOption) => void;
    expandAll?: boolean
}

const Menu = (props: MenuProps) => {
    const navigate = useNavigate();

    const menuMap = useMemo(
        () => menuItemsToFlattenMap(props.items),
        [props.items]
    );

    const onInternalItemClick = useCallback((key: string) => {
        const item = menuMap.get(key);
        if (!item) return;

        if (!props.onItemClick) {
            navigate(item.url);
            return;
        }

        props.onItemClick(item);
    }, [menuMap, navigate, props]);

    return (
        <AntMenu
            inlineIndent={1 * getRemUnit()}
            items={toAntMenuItems(props.items)}
            onClick={(e) => onInternalItemClick(e.key)}
            className={props.className}
            defaultOpenKeys={props.expandAll ? getAllMenuOptionKeys(props.items) : undefined}
            mode="inline"
        />
    );
};

export default Menu;
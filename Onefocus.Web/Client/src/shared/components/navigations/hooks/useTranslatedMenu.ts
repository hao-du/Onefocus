import { useLocale } from '../../../hooks';
import { useMemo, useCallback } from 'react';
import { MenuItem } from 'primereact/menuitem';

export function useTranslatedMenuItems(items?: MenuItem[]): MenuItem[] {
    const { translate } = useLocale();

    const translateItems = useCallback((menu: MenuItem[] | MenuItem[][]): MenuItem[] | MenuItem[][] => {
        if (Array.isArray(menu[0])) {
            return (menu as MenuItem[][]).map(group => translateItems(group) as MenuItem[]);
        }

        return (menu as MenuItem[]).map(item => ({
            ...item,
            label: item.label ? translate(item.label) : undefined,
            items: item.items ? translateItems(item.items) : undefined
        }));
    }, [translate]);

    return useMemo(() => {
        if (!items) return [];
        const translated = translateItems(items);
        return Array.isArray(translated[0])
            ? (translated as MenuItem[][]).flat()
            : (translated as MenuItem[]);
    }, [items, translateItems]);
}
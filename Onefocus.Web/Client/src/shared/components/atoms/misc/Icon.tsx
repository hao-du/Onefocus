
import { Save, Search, User } from 'lucide-react';
import type { ClassNameProps, IdentityProps } from '../../../props/BaseProps';
import useTheme from '../../../hooks/theme/useTheme';
import { SizeType, StateColorType } from '../../../types';

const ICONS = {
    search: Search,
    save: Save,
    user: User,
};

interface IconProps extends IdentityProps, ClassNameProps {
    name: 'search' | 'save' | 'user';
    size?: SizeType;
    type?: StateColorType
}

export default function Icon({
    size = 'small',
    ...props
}: IconProps) {
    const { cssClasses } = useTheme();
    const classes = [
        cssClasses.icon[size],
        props.type && `one-${props.type}-text`,
        props.className,
    ]
        .filter(Boolean)
        .join(' ');

    const Icon = ICONS[props.name];
    if (!Icon) return null;

    return (
        <Icon
            id={props.id}
            key={props.key}
            className={classes}
            name={props.name}
        />
    );
}
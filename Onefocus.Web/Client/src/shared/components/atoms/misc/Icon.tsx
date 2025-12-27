
import { DynamicIcon } from 'lucide-react/dynamic';
import type { ClassNameProps, IdentityProps } from '../../../props/BaseProps';
import useTheme from '../../../hooks/theme/useTheme';

interface IconProps extends IdentityProps, ClassNameProps {
    name: 'save' | 'loader';
    size?: 'small' | 'medium' | 'large'
    type?: 'primary' | 'success' | 'info' | 'warning' | 'error'
}

export default function Icon({ size = 'small',
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


    return (
        <DynamicIcon
            id={props.id}
            key={props.key}
            className={classes}
            name={props.name}
        />
    );
}
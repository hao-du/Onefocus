
import { BookUser, CircleDollarSign, EllipsisVertical, Gauge, HandCoins, House, Landmark, LogIn, Menu, Save, Search, Settings2, User, Users, Wallet } from 'lucide-react';
import type { ClassNameProps, IdentityProps } from '../../../props/BaseProps';
import useTheme from '../../../hooks/theme/useTheme';
import { SizeType, StateColorType } from '../../../types';

const ICONS = {
    //actions
    search: Search,
    save: Save,
    hambugger: Menu,
    login: LogIn,
    ellipsis: EllipsisVertical,
    //features
    house: House,
    wallet: Wallet,
    membership: Users,
    //sub-features
    user: User,
    dashboard: Gauge,
    transaction: HandCoins,
    bank: Landmark,
    currency: CircleDollarSign,
    counterParty: BookUser,
    //settings
    setting: Settings2,
};

interface IconProps extends IdentityProps, ClassNameProps {
    name: 'search' | 'save' | 'hambugger' | 'login' | 'ellipsis' |
    'user' | 'house' | 'wallet' | 'membership' | 'dashboard' | 'transaction' | 'bank' | 'currency' | 'counterParty' |
    'setting';
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
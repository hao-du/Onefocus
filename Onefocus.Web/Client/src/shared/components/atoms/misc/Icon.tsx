
import { BookUser, ChevronLeft, ChevronRight, CircleDollarSign, CreditCard, EllipsisVertical, Funnel, Gauge, Ghost, HandCoins, House, Landmark, LogIn, Menu, Pencil, Plus, RefreshCw, RotateCcw, Save, Search, SearchCheck, Settings2, User, Users, Wallet } from 'lucide-react';
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
    filter: Funnel,
    add: Plus,
    edit: Pencil,
    apply: SearchCheck,
    reset: RotateCcw,
    sync: RefreshCw,
    //settings
    setting: Settings2,
    ghost: Ghost,
    chevronLeft: ChevronLeft,
    chevronRight: ChevronRight,
    //features
    house: House,
    wallet: Wallet,
    membership: Users,
    //membership-features
    user: User,
    //wallet-features
    dashboard: Gauge,
    transaction: HandCoins,
    bank: Landmark,
    currency: CircleDollarSign,
    counterParty: BookUser,
    cashFlow: Landmark,
    bankAccount: CreditCard,
    peetTransfer: BookUser,
    currencyExchange: CircleDollarSign,
};

interface IconProps extends IdentityProps, ClassNameProps {
    name: 'search' | 'save' | 'hambugger' | 'login' | 'ellipsis' | 'filter' | 'add' | 'edit' | 'apply' | 'reset' | 'sync' |
    'setting' | 'ghost' | 'chevronLeft' | 'chevronRight' |
    'house' | 'wallet' | 'membership' |
    'user' |
    'dashboard' | 'transaction' | 'bank' | 'currency' | 'counterParty' | 'cashFlow' | 'bankAccount' | 'peetTransfer' | 'currencyExchange';
    size?: SizeType | 'xLarge' | 'xxLarge';
    type?: StateColorType
}

const Icon = ({ size = 'small', ...props }: IconProps) => {
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

export default Icon;
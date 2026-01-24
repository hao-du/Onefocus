import { MenuOption } from "./options/MenuOption";
import Icon from './components/atoms/misc/Icon';

export const MenuItems: MenuOption[] = [
    {
        key: '1',
        label: 'Home',
        url: '/',
        icon: <Icon name="house" />,
    },
    {
        key: '2',
        label: 'Wallet',
        icon: <Icon name="wallet" />,
        children: [
            {
                key: '2.1',
                label: 'Dashboard',
                url: '/wallet/dashboard',
                icon: <Icon name="dashboard" />,
            },
            {
                key: '2.2',
                label: 'Transactions',
                url: '/wallet/transactions',
                icon: <Icon name="transaction" />,
            },
            {
                type: 'divider',
            },
            {
                type: 'group',
                label: 'Wallet settings',
                children: [
                    {
                        key: '2.3',
                        label: 'Banks',
                        url: '/wallet/bank',
                        icon: <Icon name="bank" />,
                    },
                    {
                        key: '2.4',
                        label: 'Currencies',
                        url: '/wallet/currency',
                        icon: <Icon name="currency" />,
                    },
                    {
                        key: '2.5',
                        label: 'Counterparties',
                        url: '/wallet/counterparty',
                        icon: <Icon name="counterParty" />,
                    },
                ]
            },
        ],
    },
    {
        key: '3',
        label: 'Membership',
        url: '/membership',
        icon: <Icon name="membership" />,
        children: [
            {
                key: '3.1',
                label: 'Users',
                url: '/membership/user',
                icon: <Icon name="user" />,
            }
        ],
    },
    {
        key: '4',
        label: 'Settings',
        icon: <Icon name="setting" />,
        url: '/home/settings'
    }
];
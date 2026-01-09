import { MenuOption } from '../../options/MenuOption';
import { ChildrenProps } from '../../props/BaseProps';
import { ActionOption } from '../../options/ActionOption';
import Icon from '../atoms/misc/Icon';
import DefaultTemplate from '../templates/DefaultTemplate';

const menuItems: MenuOption[] = [
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
                url: '/wallet',
                icon: <Icon name="dashboard" />,
            },
            {
                key: '2.2',
                label: 'List',
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

interface DefaultLayoutProps extends ChildrenProps {
    title?: string;
    actions?: ActionOption[];
    showPrimaryButton?: boolean;
    singleCard?: boolean;
};

const DefaultLayout = (props: DefaultLayoutProps) => {
    return (
        <DefaultTemplate
            title={props.title}
            menuOptions={menuItems}
            showPrimaryButton
            workSpaceActions={props.actions}
            singleCard={props.singleCard}
        >
            {props.children}
        </DefaultTemplate>
    );
};

export default DefaultLayout;
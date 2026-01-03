import DefaultLayout from './shared/components/templates/DefaultLayout';
import Button from './shared/components/atoms/buttons/Button';
import Icon from './shared/components/atoms/misc/Icon';
import useWindows from './shared/hooks/windows/useWindows';
import { MenuOption } from './shared/options/MenuOption';
import Card from './shared/components/molecules/panels/Card';
import ExtraInfo from './shared/components/atoms/typography/ExtraInfo';

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


const App = () => {
    const { showToast } = useWindows();

    return (
        <DefaultLayout menuOptions={menuItems} title="Transaction list">
            <Card
                title='First Card'
                body={
                    <ExtraInfo text="This is the first card of layout" />
                }
                rightActions={
                    <Button type="primary" text="Click me" icon={<Icon name='save' />} onClick={() => {
                        showToast({
                            title: "Test title",
                            description: "Test description",
                            icon: <Icon name='save' size='middle' type='warning' />,
                            severity: 'success',
                            canBeExpired: false
                        })
                    }} />
                }
            />

        </DefaultLayout>
    );
}

export default App;

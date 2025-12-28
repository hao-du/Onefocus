import DefaultLayout from './shared/components/templates/DefaultLayout';
import Button from './shared/components/atoms/buttons/Button';
import Icon from './shared/components/atoms/misc/Icon';
import useWindows from './shared/hooks/windows/useWindows';

const App = () => {
    const { showToast } = useWindows();

    return (
        <DefaultLayout>
            <h1>
                <Button type="primary" text="Click me" icon={<Icon name='save' />} onClick={() => {
                    showToast({
                        title: "Test title",
                        description: "Test description",
                        icon: <Icon name='save' size='middle' type='warning' />,
                        severity: 'success',
                        canBeExpired: false
                    })
                }} />
            </h1>
        </DefaultLayout>
    );
}

export default App;

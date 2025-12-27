import DefaultLayout from './shared/components/templates/DefaultLayout';
import Button from './shared/components/atoms/buttons/Button';
import Icon from './shared/components/atoms/misc/Icon';
import useTheme from './shared/hooks/theme/useTheme';
import useWindows from './shared/hooks/windows/useWindows';

const App = () => {
    const { cssClasses } = useTheme();
    const { showToast } = useWindows();

    return (
        <DefaultLayout>
            <h1 className={cssClasses.testH1}>
                <Button type="primary" text="Click me" icon={<Icon name='save' />} onClick={() => {
                    showToast({
                        title: "Test title",
                        description: "Test description",
                        icon: <Icon name='save' size='medium' type='warning' />,
                        severity: 'success',
                        canBeExpired: false
                    })
                }} />
            </h1>
        </DefaultLayout>
    );
}

export default App;

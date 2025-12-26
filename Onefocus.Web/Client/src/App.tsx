import DefaultLayout from './shared/components/templates/DefaultLayout';
import Button from './shared/components/atoms/buttons/Button';
import Icon from './shared/components/atoms/misc/Icon';
import useTheme from './shared/hooks/theme/useTheme';

export default function App() {
    const { cssClasses } = useTheme();

    return (
        <DefaultLayout>
            <h1 className={cssClasses.testH1}>
                <Button type="primary" text="Click me" icon={<Icon name='save' />} />
            </h1>
        </DefaultLayout>
    );
}

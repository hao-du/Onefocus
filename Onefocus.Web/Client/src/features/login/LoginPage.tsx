import Card from "../../shared/components/atoms/panels/Card";
import Space from "../../shared/components/atoms/panels/Space";
import PageTitle from "../../shared/components/atoms/Typography/PageTitle";
import useTheme from "../../shared/hooks/theme/useTheme";
import { joinClassNames } from "../../shared/utils";
import LoginForm from "./LoginForm";


const LoginPage = () => {
    const { cssClasses } = useTheme();

    return (
        <div className={joinClassNames(
            cssClasses.size.height.dynamic,
            cssClasses.flex.center,
            cssClasses.padding.default,
            cssClasses.background.layout
        )}>
            <Space vertical className={joinClassNames(cssClasses.size.width.full, 'justify-center max-w-xl', 'md:w-2/3 xl:w-1/3')}>
                <PageTitle title="Login" align="center" />
                <Card >
                    <LoginForm />
                </Card>
            </Space>
        </div >
    );
};

export default LoginPage;
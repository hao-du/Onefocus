import Space from "../../shared/components/atoms/panels/Space";
import useTheme from "../../shared/hooks/theme/useTheme";
import { joinClassNames } from "../../shared/utils";
import LoginForm from "./LoginForm";
import PageTitle from "../../shared/components/atoms/typography/PageTitle";
import Section from "../../shared/components/atoms/panels/Section";
import ExtraInfo from "../../shared/components/atoms/typography/ExtraInfo";


const LoginPage = () => {
    const { cssClasses } = useTheme();

    return (
        <div className={joinClassNames(
            'flex-col gap-y-3',
            cssClasses.size.height.dynamic,
            cssClasses.flex.center,
            cssClasses.padding.default,
            cssClasses.background.layout
        )}>
            <Space size="large" vertical className={joinClassNames(cssClasses.size.width.full, 'xl:w-2/3 xxl:w-1/5', 'justify-center sm:max-w-md')}>
                <Section justify="center">
                    <PageTitle title="Onefocus" align="center" />
                    <ExtraInfo text="Please enter your details to sign in." align="center" />
                </Section>
                <LoginForm />
            </Space>
            <Section justify="center">
                <ExtraInfo text="© 2026 Crafted with care by Hao Du." align="center" />
            </Section>
        </div >
    );
};

export default LoginPage;
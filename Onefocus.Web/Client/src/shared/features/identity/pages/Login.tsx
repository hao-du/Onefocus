import { SingleContentLayout } from '../../../components/layouts';
import { Fieldset } from '../../../components/controls/panels';
import LoginForm from '../components/LoginForm';

const Login = () => {
    return (
        <SingleContentLayout alignCenter={true} justifyContentCenter={true}>
            <p><span className="text-6xl font-normal text-primary">Test</span>
            </p>
            <Fieldset title="Login" className="pt-4 of-center-panel-width">
                <LoginForm />
            </Fieldset>
        </SingleContentLayout>
    );
};

export default Login;
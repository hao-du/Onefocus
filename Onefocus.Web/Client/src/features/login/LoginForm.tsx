import { SubmitHandler, useForm } from 'react-hook-form';
import useWindows from '../../shared/hooks/windows/useWindows';
import useLogin from './services/useLogin';
import FormText from '../../shared/components/molecules/forms/FormText';
import FormPassword from '../../shared/components/molecules/forms/FormPassword';
import Button from '../../shared/components/atoms/buttons/Button';
import Form from '../../shared/components/molecules/forms/Form';
import Icon from '../../shared/components/atoms/misc/Icon';
import Card from '../../shared/components/molecules/panels/Card';

interface LoginFormInput {
    userName: string,
    password: string
}

const LoginForm = () => {
    const { loginAsync, isPending } = useLogin();
    const { showResponseToast } = useWindows();

    const { control, handleSubmit } = useForm<LoginFormInput>({
        defaultValues: {
            userName: undefined,
            password: undefined
        }
    });

    const onSubmit: SubmitHandler<LoginFormInput> = async (data) => {
        const response = await loginAsync({ email: data.userName, password: data.password });
        if (response.status != 200) {
            showResponseToast(response);
        }
    };

    return (
        <Form layout="vertical" onSubmit={handleSubmit(onSubmit)}>
            <Card
                title="Signin" titleAlign="center"
                body={
                    <>
                        <FormText name="userName" control={control} label="Username" className="w-full" autoComplete="username" rules={{
                            required: 'User name is required.'
                        }} />
                        <FormPassword name="password" control={control} label="Password" className="w-full" autoComplete="current-password" rules={{
                            required: 'Password is required.'
                        }} />
                    </>
                }
                actions={
                    <Button htmlType="submit" text="Sign in" isPending={isPending} block icon={<Icon name="login" size="small" />} />
                }
            />
        </Form>
    );
};

export default LoginForm;
import { SubmitHandler, useForm } from 'react-hook-form';
import useWindows from '../../shared/hooks/windows/useWindows';
import useLogin from './services/useLogin';
import FormText from '../../shared/components/organisms/forms/FormText';
import FormPassword from '../../shared/components/organisms/forms/FormPassword';
import Button from '../../shared/components/atoms/buttons/Button';

interface LoginFormInput {
    userName: string,
    password: string
}

const LoginForm = () => {
    const { loginAsync, isPending } = useLogin();
    const { showResponseToast } = useWindows();

    const { control, handleSubmit } = useForm<LoginFormInput>({
        defaultValues: {
            userName: 'kevindu1986@gmail.com',
            password: 'Asd@123'
        }
    });

    const onSubmit: SubmitHandler<LoginFormInput> = async (data) => {
        const response = await loginAsync({ email: data.userName, password: data.password });
        if (response.status != 200) {
            showResponseToast(response);
        }
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <FormText name="userName" control={control} label="Username" className="w-full" rules={{
                required: 'User name is required.'
            }} />
            <FormPassword name="password" control={control} label="Password" className="w-full" rules={{
                required: 'Password is required.'
            }} />
            <Button htmlType="submit" text="Log in" isPending={isPending} />
        </form>
    );
};

export default LoginForm;
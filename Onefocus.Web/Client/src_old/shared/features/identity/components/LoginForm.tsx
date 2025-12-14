import { SubmitHandler, useForm } from 'react-hook-form';
import { Button } from '../../../components/controls/buttons';
import { Password, Text } from '../../../components/controls';
import { useLogin } from '../services';
import LoginFormInput from './interfaces/LoginFormInput';
import { useWindows } from '../../../components/hooks';

const LoginForm = () => {
    const { onLoginAsync, isPending } = useLogin();
    const { showResponseToast } = useWindows();

    const { control, handleSubmit } = useForm<LoginFormInput>({
        defaultValues: {
            userName: 'kevindu1986@gmail.com',
            password: 'Asd@123'
        }
    });

    const onSubmit: SubmitHandler<LoginFormInput> = async (data) => {
        const response = await onLoginAsync({ email: data.userName, password: data.password });
        if(response.status != 200) {
            showResponseToast(response);
        }
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <Text name="userName" control={control} label="Username" className="w-full" autoComplete="username" rules={{
                required: 'User name is required.'
            }} />
            <Password name="password" control={control} label="Password" className="w-full" rules={{
                required: 'Password is required.'
            }} />
            <Button type="submit" className="mb-5" label="Sign in" icon="pi-lock" isPending={isPending} />
        </form>
    );
};

export default LoginForm;
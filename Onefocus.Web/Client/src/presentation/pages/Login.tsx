import {SubmitHandler, useForm} from 'react-hook-form';
import {useLogin} from '../../application/authentication';
import {SingleContentLayout} from '../layouts';
import {Fieldset} from '../components/controls/panels';
import {Password, Text} from '../components/form-controls';
import {Button} from '../components/controls/buttons';
import {useWindows} from '../components/hooks/useWindows';

interface IFormInput {
    userName: string,
    password: string
}

export const Login = () => {
    const {onLoginAsync, isPending} = useLogin();
    const {showResponseToast} = useWindows();

    const {control, handleSubmit} = useForm<IFormInput>({
        defaultValues: {
            userName: 'kevindu1986@gmail.com',
            password: 'hao123'
        }
    });

    const onSubmit: SubmitHandler<IFormInput> = async (data) => {
        const response = await onLoginAsync({email: data.userName, password: data.password});
        showResponseToast(response, '');
    };

    return (
        <SingleContentLayout alignCenter={true} justifyContentCenter={true}>
            <p><span className="text-6xl font-normal text-primary">Onefocus</span><span className="text-black-alpha-40"> by HaoDu</span>
            </p>
            <Fieldset title="Login" className="pt-4 of-center-panel-width">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <Text name="userName" control={control} label="Username" className="w-full" autoComplete="username" rules={{
                        required: 'User name is required.'
                    }} />
                    <Password name="password" control={control} label="Password" className="w-full" rules={{
                        required: 'Password is required.'
                    }} />
                    <Button type="submit" className="mb-5" label="Sign in" icon="pi-lock" isPending={isPending}/>
                </form>
            </Fieldset>
        </SingleContentLayout>
    );
};

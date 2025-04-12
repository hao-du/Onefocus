import useLogin from "../../application/authentication/useLogin";
import Button from "../components/controls/buttons/Button";
import SingleContentLayout from "../layouts/SingleContentLayout";
import Fieldset from "../components/controls/panels/Fieldset";
import {SubmitHandler, useForm} from "react-hook-form";
import {Password} from "../components/form-controls/inputs/Password";
import {Text} from "../components/form-controls/inputs/Text";

interface IFormInput {
    userName: string,
    password: string
}

const Login = () => {
    const {mutateAsync, isPending} = useLogin();

    const { control, handleSubmit } = useForm<IFormInput>({ defaultValues: {
        userName: 'kevindu1986@gmail.com',
        password: 'hao123'
    }});

    const onSubmit: SubmitHandler<IFormInput> = async (data) => {
        return await mutateAsync({ email: data.userName, password: data.password });
    };

    return (
        <SingleContentLayout alignCenter={true} justifyContentCenter={true}>
            <p><span className="text-6xl">Wallet</span><span> by Onefocus</span></p>
            <Fieldset title="Login" className="pt-4">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <Text name="userName" control={control} label="Username"></Text>
                    <Password name="password" control={control} label="Password"></Password>
                    <Button type="submit" className="mb-5" label="Sign in" icon="pi-lock" isPending={isPending} />
                </form>
            </Fieldset>
        </SingleContentLayout>
    );
};

export default Login;

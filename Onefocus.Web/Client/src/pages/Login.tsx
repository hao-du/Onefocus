import {useMutation} from "@tanstack/react-query";
import {useNavigate} from "react-router";
import {useAuth} from "../hooks/authentication/useAuth.tsx";
import useAuthenticationApi from "../api/authentication/useAuthenticationApi.ts";
import {useState} from "react";
import Button from "../components/atoms/buttons/Button.tsx";
import Text from "../components/atoms/inputs/Text.tsx";
import Password from "../components/atoms/inputs/Password.tsx";
import DefaultLayout from "../components/layouts/DefaultLayout.tsx";
import Fieldset from "../components/atoms/panels/Fieldset.tsx";

const Login = () => {
    const {setToken} = useAuth();
    const {authenticate} = useAuthenticationApi();
    const navigate = useNavigate();

    const [userName, setUserName] = useState<string>('kevindu1986@gmail.com');
    const [password, setPasword] = useState<string>('hao123');

    const {mutateAsync, isPending} = useMutation({
        mutationFn: async () => {
            const response = await authenticate({email: userName, password: password});
            if (response.status === 200) {
                setToken(response.value.token);
                navigate('/wallet');
            } else {
                setToken(null);
            }
        }
    });

    return (
        <DefaultLayout alignCenter={true} justifyContentCenter={true}>
            <p><span className="text-6xl">Wallet</span><span> by Onefocus</span></p>
            <Fieldset title="Login" className="pt-4">
                <Text id="username" label="Username" value={userName} onChange={(e: React.ChangeEvent<HTMLInputElement>) => setUserName(e.target.value)}></Text>
                <Password id="password" label="Password" value={password} onChange={(e) => setPasword(e.target.value)}></Password>
                <Button className="mb-5" label="Sign in" icon="pi-lock" isPending={isPending} onClick={async () => { await mutateAsync(); }}/>
            </Fieldset>
        </DefaultLayout>
    );
};

export default Login;

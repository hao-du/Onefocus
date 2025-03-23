import {useMutation} from "@tanstack/react-query";
import {useNavigate} from "react-router";
import {useAuth} from "../hooks/authentication/useAuth.tsx";
import useAuthenticationApi from "../api/authentication/useAuthenticationApi.ts";
import {Fieldset} from "primereact/fieldset";
import {FloatLabel} from "primereact/floatlabel";
import {InputText} from "primereact/inputtext";
import {Password} from "primereact/password";
import {useState} from "react";
import {Button} from "primereact/button";

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
        <>
            <div className="h-screen flex align-items-center justify-content-center">
                <div className="flex flex-column align-items-center">
                    <p><span className="text-6xl">Wallet</span><span> by Onefocus</span></p>
                    <Fieldset legend="Login" className="pt-4">
                        <FloatLabel className="mb-5">
                            <InputText id="username" value={userName} onChange={(e: React.ChangeEvent<HTMLInputElement>) => setUserName(e.target.value)} />
                            <label htmlFor="username">Username</label>
                        </FloatLabel>
                        <FloatLabel className="mb-5">
                            <Password inputId="password" feedback={false} value={password} onChange={(e) => setPasword(e.target.value)} />
                            <label htmlFor="password">Password</label>
                        </FloatLabel>
                        {<Button className="mb-5" label="Sign in" icon={'pi' + (isPending ? ' pi-spin pi-spinner' : ' pi-lock')} disabled={isPending} onClick={async () => { await mutateAsync(); }}/>}
                    </Fieldset>
                </div>
            </div>
        </>
    );
};

export default Login;

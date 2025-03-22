import {useMutation} from "@tanstack/react-query";
import {redirect} from "react-router";
import {useAuth} from "../hooks/authentication/useAuth";
import useAuthenticationApi from "../api/authentication/useAuthenticationApi";

const Login = () => {
    const {setToken} = useAuth();
    const {authenticate} = useAuthenticationApi();

    const {mutateAsync} = useMutation({
        mutationFn: async () => {
            const response = await authenticate({email: 'kevindu1986@gmail.com', password: 'hao123'});
            if (response.status === 200) {
                setToken(response.value.token);
                redirect('/');
            } else {
                setToken(null);
            }
        }
    });

    const onLogin = async () => {
        await mutateAsync();
    };

    return (
        <>
            <button onClick={onLogin}>Login</button>
        </>
    );
};

export default Login;

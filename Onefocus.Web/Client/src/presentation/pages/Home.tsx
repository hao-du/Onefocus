import {useNavigate} from 'react-router';
import {Button} from '../components/controls/buttons';

export const Home = () => {
    const navigate = useNavigate();

    return (
        <div className="h-screen flex align-items-center justify-content-center">
            <div className="flex flex-column">
                <p className="text-5xl">Welcome to Onefocus!</p>
                <Button link label="Click here to open your Wallet" onClick={() => {
                    navigate('/wallet');
                }}/>
            </div>
        </div>
    );
};
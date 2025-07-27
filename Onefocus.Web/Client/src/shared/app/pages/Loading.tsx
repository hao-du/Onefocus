import { ProgressSpinner } from "../../components/indicators";

const Loading = () => {
    return (
        <div className="h-screen flex align-items-center justify-content-center">
            <div className="flex flex-column">
                <p className="text-5xl">Application is starting...</p>
                <ProgressSpinner style={{width: '50px', height: '50px'}}/>
            </div>
        </div>
    );
};

export default Loading;
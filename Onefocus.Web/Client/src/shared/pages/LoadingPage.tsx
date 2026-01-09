import useTheme from "../../shared/hooks/theme/useTheme";
import { joinClassNames } from "../../shared/utils";
import Loading from "../components/atoms/misc/Loading";
import ExtraInfo from "../components/atoms/typography/ExtraInfo";


const LoadingPage = () => {
    const { cssClasses } = useTheme();

    return (
        <div className={joinClassNames(
            'flex-col gap-y-3',
            cssClasses.size.height.dynamic,
            cssClasses.flex.center,
            cssClasses.padding.default,
            cssClasses.background.layout
        )}>
            <Loading size="large" className="justify-center" />
            <ExtraInfo text="Please wait a moment..." align="center" />
        </div >
    );
};

export default LoadingPage;
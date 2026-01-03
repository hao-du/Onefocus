import { Typography } from "antd";
import { ClassNameProps, StyleProps } from "../../../props/BaseProps";
import { joinClassNames } from "../../../utils";
import useTheme from "../../../hooks/theme/useTheme";
import { HAlignType } from "../../../types";

interface AppTitleProps extends ClassNameProps, StyleProps {
    title: string
    align?: HAlignType
}

const AppTitle = ({
    align = 'left',
    ...props
}: AppTitleProps) => {
    const { cssClasses } = useTheme();

    return (
        <Typography.Title
            level={1}
            className={joinClassNames(props.className, cssClasses.text[align])}
            style={props.style ?? { margin: 0 }}
        >
            {props.title}
        </Typography.Title>
    );
};

export default AppTitle;
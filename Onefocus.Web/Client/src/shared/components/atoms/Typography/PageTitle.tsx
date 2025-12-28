import { Typography } from "antd";
import { ClassNameProps } from "../../../props/BaseProps";
import { joinClassNames } from "../../../utils";
import useTheme from "../../../hooks/theme/useTheme";
import { HAlignType } from "../../../types";

interface PageTitleProps extends ClassNameProps {
    title: string
    align?: HAlignType
}

const PageTitle = ({
    align = 'left',
    ...props
}: PageTitleProps) => {
    const { cssClasses } = useTheme();

    return (
        <Typography.Title
            level={1}
            className={joinClassNames(props.className, cssClasses.text[align])}
            style={{ marginBottom: 0 }}
        >
            {props.title}
        </Typography.Title>
    );
};

export default PageTitle;
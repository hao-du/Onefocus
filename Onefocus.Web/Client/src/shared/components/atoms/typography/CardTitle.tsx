import { Typography } from "antd";
import { ClassNameProps, StyleProps } from "../../../props/BaseProps";
import { joinClassNames } from "../../../utils";
import useTheme from "../../../hooks/theme/useTheme";
import { HAlignType } from "../../../types";

interface CardTitleProps extends ClassNameProps, StyleProps {
    title: string
    align?: HAlignType
}

const CardTitle = ({
    align = 'left',
    ...props
}: CardTitleProps) => {
    const { cssClasses } = useTheme();

    return (
        <Typography.Title
            level={3}
            className={joinClassNames(props.className, cssClasses.text[align])}
            style={props.style ?? { margin: 0 }}
        >
            {props.title}
        </Typography.Title>
    );
};

export default CardTitle;
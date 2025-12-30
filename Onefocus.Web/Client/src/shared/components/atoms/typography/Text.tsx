import { Typography } from "antd";
import { ClassNameProps } from "../../../props/BaseProps";
import { joinClassNames } from "../../../utils";
import { HAlignType } from "../../../types";
import useTheme from "../../../hooks/theme/useTheme";

interface ExtraInfoProps extends ClassNameProps {
    text: string;
    align?: HAlignType;
}

const ExtraInfo = ({
    align = 'left',
    ...props
}: ExtraInfoProps) => {
    const { cssClasses } = useTheme();

    return (
        <Typography.Paragraph
            type="secondary"
            className={joinClassNames(props.className, cssClasses.text[align])}
            style={{ margin: 0 }}
        >
            {props.text}
        </Typography.Paragraph>
    );
};

export default ExtraInfo;
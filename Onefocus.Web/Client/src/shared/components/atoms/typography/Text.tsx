import { Typography } from "antd";
import { ClassNameProps } from "../../../props/BaseProps";
import { joinClassNames } from "../../../utils";
import { HAlignType } from "../../../types";
import useTheme from "../../../hooks/theme/useTheme";

interface TextProps extends ClassNameProps {
    text: string;
    align?: HAlignType;
    strong?: boolean
}

const Text = ({
    align = 'left',
    ...props
}: TextProps) => {
    const { cssClasses } = useTheme();

    return (
        <Typography.Text
            className={joinClassNames(props.className, cssClasses.text[align])}
            strong
            style={{ margin: 0 }}
        >
            {props.text}
        </Typography.Text>
    );
};

export default Text;
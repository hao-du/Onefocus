import { useLocale } from "../../../hooks/locale/LocaleContext";
import { BaseHtmlProps, BaseIdentityProps } from "../../../props/BaseProps";

interface TextOnlyProps extends BaseIdentityProps, BaseHtmlProps {
    value: string;
};

export const Span = (props: TextOnlyProps) => {
    const {translate} = useLocale();

    return (
        <span
            id={props.id}
            key={props.key}
            className={props.className}
        >
            { translate(props.value) }
        </span>
    );
};
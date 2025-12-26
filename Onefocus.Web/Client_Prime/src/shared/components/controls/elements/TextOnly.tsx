import { useLocale } from "../../../hooks";

type TextOnlyProps = {
    value: string
};

const TextOnly = (props: TextOnlyProps) => {
    const {translate} = useLocale();

    return (
        <>
            { translate(props.value) }
        </>
    );
}

export default TextOnly;
import { Tag as PrimeTag } from "primereact/tag";
import { BaseHtmlProps, BaseIdentityProps } from "../../../props/BaseProps";

interface TagProps extends BaseIdentityProps, BaseHtmlProps {
    value?: React.ReactNode;
    severity?: 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast';
};

export const Tag = (props: TagProps) => {
    return (
        <PrimeTag
            id={props.id}
            key={props.key}
            value={props.value}
            severity={props.severity ?? 'success'}
            className={props.className}
        />
    )
};
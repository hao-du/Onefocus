import { Tag as PrimeTag } from "primereact/tag";
import { BaseProps } from "../../props";

type TagProps = BaseProps & {
    value?: React.ReactNode;
    severity?: 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast';
}

const Tag = (props: TagProps) => {
    return (
        <PrimeTag
            value={props.value}
            severity={props.severity ?? 'success'}
            className={props.className}
        />
    )
}

export default Tag;
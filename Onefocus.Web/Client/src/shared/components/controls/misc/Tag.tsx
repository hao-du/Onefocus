import { Tag as PrimeTag } from "primereact/tag";

type TagProps = {
    value?: React.ReactNode;
    severity?: 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast';
}

const Tag = (props: TagProps) => {
    return (
        <PrimeTag
            value={props.value}
            severity={props.severity ?? 'success'}
        />
    )
}

export default Tag;
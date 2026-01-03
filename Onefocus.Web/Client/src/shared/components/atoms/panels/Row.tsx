import { Row as AntRow } from "antd";
import { ChildrenProps, ClassNameProps } from "../../../props/BaseProps";
import { JustifyContentType } from "../../../types";

interface RowProps extends ChildrenProps, ClassNameProps {
    justify?: JustifyContentType
}

const Row = (props: RowProps) => {
    return (
        <AntRow
            justify={props.justify}
            className={props.className}
        >
            {props.children}
        </AntRow>
    );
}

export default Row;
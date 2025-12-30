import { Row as AntRow } from "antd";
import { ChildrenProps } from "../../../props/BaseProps";
import { JustifyContentType } from "../../../types";

interface RowProps extends ChildrenProps {
    justify?: JustifyContentType
}

const Row = (props: RowProps) => {
    return (
        <AntRow
            justify={props.justify}
        >
            {props.children}
        </AntRow>
    );
}

export default Row;
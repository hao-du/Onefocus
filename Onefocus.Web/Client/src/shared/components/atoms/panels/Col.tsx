import { Col as AntCol } from "antd";
import { ChildrenProps, ClassNameProps } from "../../../props/BaseProps";
import { GridSizeType } from "../../../types";

interface ColProps extends ChildrenProps, ClassNameProps {
    flex?: number | 'auto' | 'none';
    span?: number;
    xs?: GridSizeType,
    sm?: GridSizeType,
}

const Col = (props: ColProps) => {
    return (
        <AntCol
            span={props.span}
            xs={props.xs}
            sm={props.sm}
            flex={props.flex}
            className={props.className}
        >
            {props.children}
        </AntCol>
    );
}

export default Col;
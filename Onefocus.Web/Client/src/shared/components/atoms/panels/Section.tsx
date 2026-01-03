
import { ChildrenProps, ClassNameProps } from "../../../props/BaseProps";
import { GridSizeType, JustifyContentType } from "../../../types";
import Col from "./Col";
import Row from "./Row";

interface SectionProps extends ChildrenProps, ClassNameProps {
    justify?: JustifyContentType;
    xs?: GridSizeType;
}

const Section = (props: SectionProps) => {
    return (
        <Row justify={props.justify ?? 'start'} className={props.className}>
            <Col xs={props.xs ?? 24}>
                {props.children}
            </Col>
        </Row>
    );
}

export default Section;
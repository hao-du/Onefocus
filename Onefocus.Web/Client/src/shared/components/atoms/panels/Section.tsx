
import { ChildrenProps } from "../../../props/BaseProps";
import { GridSizeType, JustifyContentType } from "../../../types";
import Col from "./Col";
import Row from "./Row";

interface SectionProps extends ChildrenProps {
    justify?: JustifyContentType;
    xs?: GridSizeType;
}

const Section = (props: SectionProps) => {
    return (
        <Row justify={props.justify ?? 'start'}>
            <Col xs={props.xs ?? 24}>
                {props.children}
            </Col>
        </Row>
    );
}

export default Section;
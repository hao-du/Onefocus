import { ActionOption } from '../../../options/ActionOption';
import { ChildrenProps } from '../../../props/BaseProps';
import Col from '../../atoms/panels/Col';
import Row from '../../atoms/panels/Row';
import Space from '../../atoms/panels/Space';
import PageTitle from '../../atoms/typography/PageTitle';
import DropdownButton from '../../molecules/buttons/DropdownButton';

interface WorkspaceProps extends ChildrenProps {
    title?: string;
    actions?: ActionOption[];
    showPrimaryAction?: boolean;
};

const Workspace = (props: WorkspaceProps) => {
    return (
        <div className="flex-1 overflow-auto p-4 pt-0 bg-(--ant-color-bg-layout)">
            {(props.title || props.actions) && (
                <Row justify="start" className="sticky top-0 z-1 py-4 bg-(--ant-color-bg-layout)">
                    <Col xs={16} className="content-center">
                        <PageTitle title={props.title ?? ''} />
                    </Col>
                    <Col xs={8} className="text-right content-center">
                        <DropdownButton actions={props.actions} showPrimaryButton={props.showPrimaryAction} />
                    </Col>
                </Row>
            )}
            <Space vertical>
                {props.children}
            </Space>
        </div >
    );
};

export default Workspace;
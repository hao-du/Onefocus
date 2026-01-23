import { ActionOption } from '../../../options/ActionOption';
import { ChildrenProps, ClassNameProps } from '../../../props/BaseProps';
import { joinClassNames } from '../../../utils';
import Col from '../../atoms/panels/Col';
import Row from '../../atoms/panels/Row';
import Space from '../../atoms/panels/Space';
import PageTitle from '../../atoms/typography/PageTitle';
import DropdownButton from '../../molecules/buttons/DropdownButton';

interface WorkspaceProps extends ChildrenProps, ClassNameProps {
    title?: string;
    actions?: ActionOption[];
    showPrimaryAction?: boolean;
};

const Workspace = (props: WorkspaceProps) => {
    return (
        <div className={joinClassNames('flex-1 pt-0 bg-(--ant-color-bg-layout) flex flex-col', props.className)}>
            {(props.title || props.actions) && (
                <Row justify="start" className="p-3 bg-(--ant-color-bg-layout)">
                    <Col xs={16} className="content-center">
                        <PageTitle title={props.title ?? ''} />
                    </Col>
                    <Col xs={8} className="text-right content-center">
                        <DropdownButton actions={props.actions} showPrimaryButton={props.showPrimaryAction} />
                    </Col>
                </Row>
            )}
            <div className="flex-1 overflow-y-auto min-h-0">
                <div className='h-0'>
                    <Space vertical className="w-full px-3 pb-3">
                        {props.children}
                    </Space>
                </div>
            </div>
        </div>
    );
};

export default Workspace;
import { ChildrenProps } from '../../../props/BaseProps';
import Section from '../../atoms/panels/Section';
import PageTitle from '../../atoms/typography/PageTitle';

interface WorkspaceProps extends ChildrenProps {
    title?: string;
};

function Workspace(props: WorkspaceProps) {
    return (
        <div className="flex-1 overflow-auto p-4 bg-(--ant-color-bg-layout)">
            {props.title && (
                <Section className="mb-4">
                    <PageTitle title={props.title} />
                </Section>
            )}
            {props.children}
        </div>
    );
};

export default Workspace;
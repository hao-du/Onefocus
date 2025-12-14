import { Splitter, SplitterPanel } from 'primereact/splitter';
import { WorkspaceActionBar } from '.';
import { ActionItem } from '../../controls';
import { useWindows } from '../../hooks';
import { BaseProps } from '../../props';

type WorkspaceLayoutProps = BaseProps & {
    title: string;
    actionItems?: ActionItem[];
    leftPanel: React.ReactNode;
    rightPanel?: React.ReactNode;
    isPending?: boolean;
};

const Workspace = (props: WorkspaceLayoutProps) => {
    const { isMobile } = useWindows();

    if (isMobile) {
        return (
            <div className="flex flex-column h-full w-full p-3">
                <WorkspaceActionBar {...props} />
                {props.leftPanel}
                {props.rightPanel}
            </div>
        );
    }

    if (props.rightPanel === undefined) {
        return (
            <div className="flex flex-column h-full w-full p-3">
                <WorkspaceActionBar {...props} />
                {props.leftPanel}
            </div>
        );
    }

    return (
        <Splitter className="border-none" style={{ height: "100%", width: "100%" }}>
            <SplitterPanel size={70} minSize={20}>
                <div className="flex flex-column h-full w-full p-3">
                    <WorkspaceActionBar {...props} />
                    {props.leftPanel}
                </div>
            </SplitterPanel>

            <SplitterPanel size={30} minSize={20}>
                {props.rightPanel}
            </SplitterPanel>
        </Splitter>
    );
}

export default Workspace;
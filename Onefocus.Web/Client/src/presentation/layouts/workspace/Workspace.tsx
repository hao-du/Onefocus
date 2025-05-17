import {Splitter, SplitterPanel} from 'primereact/splitter';
import {BaseProps} from '../../props/BaseProps';
import {SplitButtonActionItem} from '../../components/controls/buttons';
import {RightPanelProps, WorkspaceActionBar, WorkspaceRightPanel} from '.';


type WorkspaceLayoutProps = BaseProps & {
    title: string;
    actionItems?: SplitButtonActionItem[];
    leftPanel: React.ReactNode;
    rightPanelProps?: RightPanelProps;
    rightPanel: React.ReactNode;
};

export const Workspace = (props: WorkspaceLayoutProps) => {
    return (
        <div className="h-full w-full">
            {/* Desktop View */}
            <div className="hidden md:block h-full w-full">
                <Splitter className="border-none" style={{height: "100%", width: "100%"}}>
                    <SplitterPanel size={70} minSize={20}>
                        <div className="flex flex-column h-full w-full p-3">
                            <WorkspaceActionBar title={props.title} actionItems={props.actionItems}/>
                            {props.leftPanel}
                        </div>
                    </SplitterPanel>

                    <SplitterPanel size={30} minSize={20}>
                        <WorkspaceRightPanel
                            isMobileMode={false}
                            rightPanelProps={props.rightPanelProps}
                        >
                            {props.rightPanel}
                        </WorkspaceRightPanel>
                    </SplitterPanel>
                </Splitter>
            </div>

            {/* Mobile View */}
            <div className="block md:hidden h-full w-full">
                <div className="flex flex-column h-full w-full p-3">
                    <WorkspaceActionBar
                        title={props.title}
                        showMinimizedButton={!props.rightPanelProps?.viewRightPanel && props.rightPanel != null}
                        rightPanelProps={props.rightPanelProps}
                        actionItems={props.actionItems}
                    />
                    <div className="overflow-auto flex-1">
                        {props.leftPanel}
                    </div>
                    <WorkspaceRightPanel
                        isMobileMode={true}
                        rightPanelProps={props.rightPanelProps}>
                        {props.rightPanel}
                    </WorkspaceRightPanel>
                </div>
            </div>
        </div>
    );
};
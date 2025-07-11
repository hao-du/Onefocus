import {Splitter, SplitterPanel} from 'primereact/splitter';
import {BaseProps} from '../../props/BaseProps';
import {ActionItem} from '../../components/controls/buttons';
import {WorkspaceActionBar} from '.';
import {useWindows} from '../../components/hooks/useWindows';


type WorkspaceLayoutProps = BaseProps & {
    title: string;
    actionItems?: ActionItem[];
    leftPanel: React.ReactNode;
    rightPanel: React.ReactNode;
    isPending?: boolean;
};

export const Workspace = (props: WorkspaceLayoutProps) => {
    const  { isMobile } = useWindows();

    return (
        <div className="h-full w-full">
            {!isMobile && (
                <Splitter className="border-none" style={{height: "100%", width: "100%"}}>
                    <SplitterPanel size={70} minSize={20}>
                        <div className="flex flex-column h-full w-full p-3">
                            <WorkspaceActionBar {...props}/>
                            {props.leftPanel}
                        </div>
                    </SplitterPanel>

                    <SplitterPanel size={30} minSize={20}>
                        {props.rightPanel}
                    </SplitterPanel>
                </Splitter>
            )}

            {isMobile && (
                <div className="flex flex-column h-full w-full p-3">
                    <WorkspaceActionBar {...props}/>
                    <div className="overflow-auto flex-1">
                        {props.leftPanel}
                    </div>
                    {props.rightPanel}
                </div>
            )}
        </div>
    );
};
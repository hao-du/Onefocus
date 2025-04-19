import { Button } from "primereact/button";
import {SplitButton, SplitButtonProps} from "primereact/splitbutton";
import {BaseProps} from "../../props/BaseProps";
import {RightPanelProps} from "./workspace.interface";

type WorkspaceActionBarProps = BaseProps & {
    title: string
    showMinimizedButton?: boolean;
    rightPanelProps?: RightPanelProps;
    actionItems: SplitButtonProps[]
};

const WorkspaceActionBar = (props: WorkspaceActionBarProps) => {

    return (
        <div className="flex justify-content-between align-items-center mb-3">
            <h2 className="m-0 text-xl font-bold">{props.title}</h2>
            <div className="flex gap-2 align-items-center">
                {props.showMinimizedButton && (
                    <Button
                        label="Resume Editing"
                        icon="pi pi-pencil"
                        className="md:hidden"
                        onClick={() => props.rightPanelProps?.setViewRightPanel ? props.rightPanelProps.setViewRightPanel(true) : null}
                    />
                )}
                <SplitButton
                    appendTo="self"
                    label="Actions"
                    icon="pi pi-bars"
                    model={props.actionItems}
                    onClick={() => console.log("Primary Action")}
                    menuClassName="left-auto right-0"
                />
            </div>
        </div>
    );
};

export default WorkspaceActionBar;

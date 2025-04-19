import {EditPanelButton} from "./WorkspaceRightPanel";

export interface RightPanelProps {
    viewRightPanel?: boolean;
    setViewRightPanel?: React.Dispatch<React.SetStateAction<boolean>>;
    buttons?: EditPanelButton[];
}
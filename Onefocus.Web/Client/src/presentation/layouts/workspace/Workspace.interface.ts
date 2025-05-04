export interface EditPanelButton {
    onClick: (object: unknown) => void;
    id: string;
    label?: string,
    icon?: string
}

export interface RightPanelProps {
    viewRightPanel?: boolean;
    setViewRightPanel?: React.Dispatch<React.SetStateAction<boolean>>;
    buttons?: EditPanelButton[];
}
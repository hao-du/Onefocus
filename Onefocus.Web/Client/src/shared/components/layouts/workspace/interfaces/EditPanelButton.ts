export default interface EditPanelButton {
    onClick: (object: unknown) => void;
    id: string;
    label?: string,
    icon?: string
}
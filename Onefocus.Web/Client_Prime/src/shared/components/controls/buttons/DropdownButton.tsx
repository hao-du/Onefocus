import { useRef } from "react";
import { Menu } from "../menu";
import Button from "./Button";
import { ActionItem } from "../interfaces";
import { BaseButtonProps, BaseProps } from "../../props";
import { UniqueComponentId } from "primereact/utils";

type DropdownButtonRef = {
    toggle: (event: React.MouseEvent<HTMLElement>) => void;
};

type DropdownButtonProps = BaseButtonProps & BaseProps & {
    actionItems?: ActionItem[]
    isPending?: boolean;
};

const DropdownButton = (props: DropdownButtonProps) => {
    const dropdownButtonRef = useRef<DropdownButtonRef>(null);
    const uniqueComponentId = UniqueComponentId();

    return (
        <>
            <Button
                icon="pi pi-ellipsis-v"
                text={props.text}
                rounded={props.rounded}
                onClick={(e) => dropdownButtonRef.current?.toggle(e)}
                aria-haspopup
                aria-controls={`dropdown_Button_${uniqueComponentId}`}
                isPending={props.isPending}
            />
            <Menu model={props.actionItems} ref={dropdownButtonRef} id={`dropdown_Button_${uniqueComponentId}`} />
        </>
    );
}

export default DropdownButton;
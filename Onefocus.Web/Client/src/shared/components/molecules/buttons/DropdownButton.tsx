import { useRef } from "react";
import { BaseHtmlProps, BaseIdentityProps } from "../../../props/BaseProps";
import { ActionItem } from "../../atoms/options/ActionItem";
import { newGuid } from "../../../utils/formatUtils";
import { Button } from "../../atoms/buttons/Button";
import { Menu } from "../../atoms/navigations/Menu";

interface DropdownButtonProps extends BaseIdentityProps, BaseHtmlProps {
    text?: boolean;
    rounded?: boolean;
    actionItems?: ActionItem[]
    isPending?: boolean;
};

type DropdownButtonRef = {
    toggle: (event: React.MouseEvent<HTMLElement>) => void;
};

const DropdownButton = (props: DropdownButtonProps) => {
    const dropdownButtonRef = useRef<DropdownButtonRef>(null);
    const uniqueComponentId = newGuid();
   
    return (
        <>
            <Button
                id={props.id}
                key={props.key}
                icon="pi pi-ellipsis-v"
                text={props.text}
                rounded={props.rounded}
                onClick={(e) => dropdownButtonRef.current?.toggle(e)}
                aria-haspopup
                aria-controls={`dropdown_Button_${props.id ?? uniqueComponentId}`}
                isPending={props.isPending}
            />
            <Menu 
                model={props.actionItems} 
                ref={dropdownButtonRef} 
                id={`dropdown_Button_${props.id ?? uniqueComponentId}`} 
                key={`dropdown_Button_Key_${props.key ?? uniqueComponentId}`}
            />
        </>
    );
}

export default DropdownButton;
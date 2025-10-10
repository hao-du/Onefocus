import { SplitButton as PiSplitButton } from 'primereact/splitbutton';
import { ActionItem } from '../interfaces';
import { BaseButtonProps } from '../../props';
import { useLocale } from '../../../hooks';

type SplitButtonProps = BaseButtonProps & {
    appendTo?: 'self' | HTMLElement | undefined | null | (() => HTMLElement);
    menuClassName?: string;
    actionItems?: ActionItem[]
};

const SplitButton = (props: SplitButtonProps) => {
    const {translate} = useLocale();;
    const self = 'self';
    const appendTo = props.appendTo ? props.appendTo : self;
    const menuClassName = props.menuClassName ?
        props.menuClassName :
        appendTo == self ? 'left-auto right-0' : '';

    return (
        <PiSplitButton
            appendTo={appendTo}
            label={translate(props.label)}
            icon={props.isPending && props.icon ? 'pi pi-spin pi-spinner' : 'pi ' + props.icon}
            model={props.actionItems}
            disabled={props.disabled || props.isPending}
            onClick={props.onClick}
            menuClassName={menuClassName}
        />
    );
};

export default SplitButton;
import { MouseEventHandler } from 'react';
import { SplitButton as PiSplitButton } from 'primereact/splitbutton';

import { ActionItem } from '../options/ActionItem';
import { useLocale } from '../../../hooks/locale/LocaleContext';
import { BaseHtmlProps, BaseIdentityProps } from '../../../props/BaseProps';


interface SplitButtonProps extends BaseIdentityProps, BaseHtmlProps {
    label?: string;
    icon?: string;
    disabled?: boolean;
    onClick?: MouseEventHandler<HTMLButtonElement>;
    isPending?: boolean;
    appendTo?: 'self' | HTMLElement | undefined | null | (() => HTMLElement);
    menuClassName?: string;
    actionItems?: ActionItem[];
};

export const SplitButton = (props: SplitButtonProps) => {
    const {translate} = useLocale();
    const self = 'self';
    const appendTo = props.appendTo ? props.appendTo : self;
    const menuClassName = props.menuClassName ?
        props.menuClassName :
        appendTo == self ? 'left-auto right-0' : '';

    return (
        <PiSplitButton
            id={props.id}
            key={props.key}
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
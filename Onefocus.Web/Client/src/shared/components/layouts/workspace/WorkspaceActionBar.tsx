import { useEffect, useRef, useState } from 'react';
import { ActionItem, SplitButton, Button, Menu } from '../../controls';
import { BaseProps } from '../../props';
import { MenuRef } from '../../controls/menu';
import { useLocale } from '../../../hooks';

type WorkspaceActionBarProps = BaseProps & {
    title: string
    actionItems?: ActionItem[]
    isPending?: boolean;
};

const WorkspaceActionBar = (props: WorkspaceActionBarProps) => {
    const menuRef = useRef<MenuRef>(null);
    const { translate } = useLocale();
    const [hasActionItems, setHasActionItems] = useState(false);
    const [hasPrimaryAction, setHasPrimaryAction] = useState(false);
    const [primaryButton, setPrimaryButton] = useState<ActionItem | undefined>(undefined);

    useEffect(() => {
        if (props.actionItems && props.actionItems.length > 0) {
            const firstButton = props.actionItems.shift();
            setPrimaryButton(firstButton);
            if (props.actionItems.length > 0) {
                setHasActionItems(true);
            }
            if (firstButton?.command) {
                setHasPrimaryAction(true);
            }
        }
    }, [props.actionItems]);

    return (
        <div className="flex justify-content-between align-items-center mb-3">
            <h2 className="m-0 text-xl font-bold">{translate(props.title)}</h2>
            {primaryButton && (
                <div className="flex gap-2 align-items-center">
                    {hasActionItems && hasPrimaryAction && (
                        <SplitButton
                            label={translate(primaryButton.label)}
                            icon={primaryButton.icon}
                            actionItems={props.actionItems}
                            onClick={() => primaryButton.command && primaryButton.command(undefined)}
                            isPending={props.isPending}
                        />
                    )}
                    {hasActionItems && !hasPrimaryAction && (
                        <>
                            <Button
                                icon={primaryButton.icon ?? 'pi pi-chevron-down'}
                                label={translate(primaryButton.label)}
                                onClick={(e) => menuRef.current?.toggle(e)}
                                iconPos="right"
                                aria-haspopup
                                aria-controls="split_menu"
                                isPending={props.isPending}
                            />
                            <Menu model={props.actionItems} ref={menuRef} id="split_menu" />
                        </>
                    )}
                    {!hasActionItems && (
                        <Button
                            label={translate(primaryButton.label)}
                            icon={primaryButton.icon}
                            onClick={() => primaryButton.command && primaryButton.command(undefined)}
                            isPending={props.isPending}
                        />
                    )}

                </div>
            )}
        </div>
    );
};

export default WorkspaceActionBar;

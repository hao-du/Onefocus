import {ActionItem, Button, SplitButton} from '../../components/controls/buttons';
import {Menu} from '../../components/controls/menu';
import {BaseProps} from '../../props/BaseProps';
import {useRef} from 'react';

type WorkspaceActionBarProps = BaseProps & {
    title: string
    actionItems?: ActionItem[]
    isPending?: boolean;
};

export const WorkspaceActionBar = (props: WorkspaceActionBarProps) => {
    const menuRef = useRef(null);

    let primaryButton = null;
    let hasActionItems = false;
    let hasPrimaryAction = false;
    if (props.actionItems && props.actionItems.length > 0) {
        primaryButton = props.actionItems.shift();
        if (props.actionItems.length > 0) {
            hasActionItems = true;
        }
        if (primaryButton.command) {
            hasPrimaryAction = true;
        }
    }

    return (
        <div className="flex justify-content-between align-items-center mb-3">
            <h2 className="m-0 text-xl font-bold">{props.title}</h2>
            {primaryButton && (
                <div className="flex gap-2 align-items-center">
                    {hasActionItems && hasPrimaryAction && (
                        <SplitButton
                            label={primaryButton.label}
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
                                label={primaryButton.label}
                                onClick={(e) => menuRef.current?.toggle(e)}
                                iconPos="right"
                                aria-haspopup
                                aria-controls="split_menu"
                                isPending={props.isPending}
                            />
                            <Menu model={props.actionItems} ref={menuRef} id="split_menu"/>
                        </>
                    )}
                    {!hasActionItems && (
                        <Button
                            label={primaryButton.label}
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

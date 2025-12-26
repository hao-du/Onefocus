import { useRef } from 'react';
import { ActionItem, SplitButton, Button, Menu, TextOnly } from '../../controls';
import { BaseProps } from '../../props';
import { MenuRef } from '../../controls/menu';

type WorkspaceActionBarProps = BaseProps & {
    title: string
    actionItems?: ActionItem[]
    isPending?: boolean;
};

const WorkspaceActionBar = (props: WorkspaceActionBarProps) => {
    const menuRef = useRef<MenuRef>(null);

    return (
        <div className="flex justify-content-between align-items-center mb-3">
            <h2 className="m-0 text-xl font-bold"><TextOnly value={props.title} /></h2>
            <div className="flex gap-2 align-items-center">
                {props.actionItems && props.actionItems.map(actionItem => {
                    const hasActionItems = actionItem.items && actionItem.items.length > 0;
                    const hasPrimaryAction = actionItem.command;

                    return (
                        <>
                            {hasActionItems && hasPrimaryAction && (
                                <SplitButton
                                    label={actionItem.label}
                                    icon={actionItem.icon}
                                    actionItems={actionItem.items}
                                    onClick={() => actionItem.command && actionItem.command(undefined)}
                                    isPending={props.isPending}
                                    severity={actionItem.severity}
                                />
                            )}
                            {hasActionItems && !hasPrimaryAction && (
                                <>
                                    <Button
                                        icon={actionItem.icon ?? 'pi pi-chevron-down'}
                                        label={actionItem.label}
                                        onClick={(e) => menuRef.current?.toggle(e)}
                                        iconPos="right"
                                        aria-haspopup
                                        aria-controls="split_menu"
                                        isPending={props.isPending}
                                        severity={actionItem.severity}
                                    />
                                    <Menu model={actionItem.items} ref={menuRef} id="split_menu" />
                                </>
                            )}
                            {!hasActionItems && (
                                <Button
                                    label={actionItem.label}
                                    icon={actionItem.icon}
                                    onClick={() => actionItem.command && actionItem.command(undefined)}
                                    isPending={props.isPending}
                                    severity={actionItem.severity}
                                />
                            )}
                        </>
                    );

                })}
            </div>
        </div>
    );
};

export default WorkspaceActionBar;

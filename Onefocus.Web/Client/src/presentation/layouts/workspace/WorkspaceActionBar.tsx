import {Button, SplitButton, SplitButtonActionItem} from '../../components/controls/buttons';
import {BaseProps} from '../../props/BaseProps';

type WorkspaceActionBarProps = BaseProps & {
    title: string
    actionItems?: SplitButtonActionItem[]
    isPending?: boolean;
};

export const WorkspaceActionBar = (props: WorkspaceActionBarProps) => {
    let primaryButton = null;
    let hasActionItems = false;
    if (props.actionItems && props.actionItems.length > 0) {
        primaryButton = props.actionItems.shift();

        if (props.actionItems.length > 0) {
            hasActionItems = true;
        }
    }

    return (
        <div className="flex justify-content-between align-items-center mb-3">
            <h2 className="m-0 text-xl font-bold">{props.title}</h2>
            {primaryButton && (
                <div className="flex gap-2 align-items-center">
                    {hasActionItems && (
                        <SplitButton
                            label={primaryButton.label}
                            icon={primaryButton.icon}
                            actionItems={props.actionItems}
                            onClick={() => primaryButton.command && primaryButton.command(undefined)}
                            isPending={props.isPending}
                        />
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

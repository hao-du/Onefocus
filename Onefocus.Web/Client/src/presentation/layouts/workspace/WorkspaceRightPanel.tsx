import {useCallback} from 'react';
import {Button} from '../../components/controls/buttons';
import {BaseProps} from '../../props/BaseProps';
import {RightPanelProps} from '.';

type WorkspaceRightPanelProps = BaseProps & {
    isMobileMode?: boolean;
    rightPanelProps?: RightPanelProps;
    children: React.ReactNode;
};

export const WorkspaceRightPanel = (props: WorkspaceRightPanelProps) => {
    const className1 = props.isMobileMode ? 'md:hidden fixed top-0 left-0 w-full h-full bg-white z-5 shadow-5 overflow-auto flex flex-column' : 'hidden md:flex flex-column w-full'
    const className2 = props.isMobileMode ? 'flex justify-content-between align-items-center border-bottom-1 surface-border-color p-3' : 'flex justify-content-end gap-2 border-bottom-1 surface-border-color p-3'

    const renderButtons = useCallback(() => {
        let buttons = props.rightPanelProps?.buttons;
        if (!buttons) {
            buttons = []
        }

        if (props.isMobileMode && buttons.findIndex(c => c.id === 'btnMinimize') < 0) {
            buttons.push({
                onClick: () => props.rightPanelProps?.setViewRightPanel ? props.rightPanelProps.setViewRightPanel(false) : null,
                id: 'btnMinimize',
                icon: 'pi pi-window-minimize'
            });
        }

        return (
            <>
                {buttons && buttons.length > 0 &&
                    <div className={className2}>
                        {buttons.map((button) => <Button key={button.id} id={button.id} label={button.label}
                                                         onClick={button.onClick} icon={button.icon}/>)}
                    </div>
                }
            </>
        );
    }, [props.rightPanelProps?.buttons]);

    return (
        <>
            {
                (!props.isMobileMode || props.rightPanelProps?.viewRightPanel) &&
                <div className={className1}>
                    {!props.rightPanelProps || !props.rightPanelProps.viewRightPanel ? <></> : renderButtons()}

                    <div className="p-3">
                        {props.children}
                    </div>
                </div>
            }
        </>
    );
};
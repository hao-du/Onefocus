import React, { useCallback, useState } from 'react';
import { Button, SpeedDial } from '../../controls';
import { useWindows } from '../../hooks';
import { BaseProps } from '../../props';
import EditPanelButton from './interfaces/EditPanelButton';

type WorkspaceRightPanelProps = BaseProps & {
    buttons?: EditPanelButton[];
    children: React.ReactNode;
    isPending?: boolean;
};

const WorkspaceRightPanel = (props: WorkspaceRightPanelProps) => {
    const { isMobile } = useWindows();
    const [isMinimised, setIsMinimised] = useState(false);

    const renderButtons = useCallback((isMobile: boolean, isPending?: boolean, buttons?: EditPanelButton[]) => {
        if (!buttons) {
            buttons = []
        }

        if (isMobile && buttons.findIndex(c => c.id === 'btnMinimize') < 0) {
            buttons.push({
                onClick: () => {
                    setIsMinimised(true);
                },
                id: 'btnMinimize',
                icon: 'pi pi-window-minimize'
            });
        }
        return (
            <>
                {buttons && buttons.length > 0 &&
                    <div className={isMobile ?
                        'flex justify-content-between align-items-center border-bottom-1 surface-border-color p-3'
                        : 'flex justify-content-end gap-2 border-bottom-1 surface-border-color p-3'}
                    >
                        {buttons.map((button) => <Button key={button.id} id={button.id} label={button.label}
                            onClick={button.onClick} icon={button.icon}
                            isPending={isPending} />)}
                    </div>
                }
            </>
        );
    }, []);

    return (
        <>
            {isMinimised && (
                <SpeedDial onClick={() => setIsMinimised(false)} className="fixed right-1 bottom-1 w-4rem h-4rem" icon="pi-window-maximize" isPending={props.isPending} />
            )}
            {!isMinimised && (
                <div className={isMobile ? 
                    'md:hidden fixed top-0 left-0 w-full h-full bg-white z-5 shadow-5 overflow-auto flex flex-column'
                    : 'hidden md:flex flex-column w-full h-full'}
                >
                    {renderButtons(isMobile, props.isPending, props.buttons)}
                    <div className="flex-auto overflow-auto  p-3">
                        {props.children}
                    </div>
                </div>
            )}

        </>
    );
};

export default WorkspaceRightPanel;
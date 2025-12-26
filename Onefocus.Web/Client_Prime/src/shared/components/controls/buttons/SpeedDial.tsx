import { BaseButtonProps } from '../../props';
import {SpeedDial as PiSpeedDial} from 'primereact/speeddial';

type SpeedDialProps = BaseButtonProps & {
    visible?: boolean;
    type?: 'linear' | 'circle' | 'semi-circle' | 'quarter-circle';
    direction?: 'up' | 'down' | 'left' | 'right' | 'up-left' | 'up-right' | 'down-left' | 'down-right';
};

const SpeedDial = (props: SpeedDialProps) => {
    return (
        <div className={props.className ?? ''}>
            <PiSpeedDial
                type={props.type}
                showIcon={props.isPending && props.icon ? 'pi pi-spin pi-spinner' : 'pi ' + props.icon}
                disabled={props.disabled || props.isPending}
                direction={props.direction ?? 'down-right'}
                onClick={props.onClick}
            />
        </div>
    );
};

export default SpeedDial;
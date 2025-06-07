import {InputSwitch, InputSwitchChangeEvent} from 'primereact/inputswitch';
import {InputProps} from '../../../props/InputProps';

export type SwitchProps = InputProps & {
    checked: boolean;
    invalid?: boolean;
    onChange?(event: InputSwitchChangeEvent): void;
};

export const Switch = (props: SwitchProps) => {
    return (
        <div className="flex flex-column gap-2 mb-3">
            <label htmlFor={props.id}>{props.label}</label>
            <InputSwitch
                id={props.id}
                className={`${props.className} p-inputwrapper-filled`}
                checked={props.checked}
                onChange={props.onChange}
                disabled={props.isPending || props.readOnly}
                invalid={props.invalid}
            />
            {props.description && (<small className="of-text-200">{props.description}</small>)}
        </div>
    );
};
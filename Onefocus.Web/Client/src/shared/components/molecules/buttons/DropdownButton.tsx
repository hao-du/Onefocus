
import type { BaseButtonProps } from '../../../props/BaseButtonProps';
import Button from '../../atoms/buttons/Button';
import { ActionOption } from '../../../options/ActionOption';
import Dropdown from '../../atoms/buttons/Dropdown';
import Icon from '../../atoms/misc/Icon';

interface DropdownButtonProps extends BaseButtonProps {
    showPrimaryButton?: boolean;
    actions?: ActionOption[];
}

const DropdownButton = (props: DropdownButtonProps) => {
    const actions = props.actions ?? [];
    const hasPrimaryButton = props.showPrimaryButton || actions.length == 1;
    const primaryAction = hasPrimaryButton ? actions[0] : undefined;
    const dropdownActions = hasPrimaryButton ? actions.slice(1) : actions;

    return (
        <>
            {primaryAction && (
                <Button
                    key={primaryAction.key}
                    id={primaryAction.key}
                    text={primaryAction.label}
                    icon={primaryAction.icon}
                    onClick={primaryAction.command}
                    type="primary"
                    disabled={primaryAction.disabled}
                    isPending={primaryAction.isPending}
                />
            )}
            {dropdownActions?.length > 0 && (
                <Dropdown actions={dropdownActions} placement="bottomRight">
                    <Button
                        icon={<Icon name='ellipsis' />}
                        type="text"
                    />
                </Dropdown>
            )}
        </>
    );
};

export default DropdownButton;
import { ChildrenProps } from '../../../props/BaseProps';
import { joinClassNames } from '../../../utils';

interface DrawerSectionProps extends ChildrenProps {
    paddingTop?: boolean;
}

const DrawerSection = (props: DrawerSectionProps) => {

    return (
        <div className={joinClassNames('px-3', props.paddingTop ? 'pt-3' : '')}>
            {props.children}
        </div>
    );
};

export default DrawerSection;
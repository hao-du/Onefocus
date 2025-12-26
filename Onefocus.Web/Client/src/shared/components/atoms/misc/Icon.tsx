
import { DynamicIcon } from 'lucide-react/dynamic';
import type { ClassNameProps, IdentityProps } from '../../../props/BaseProps';

interface IconProps extends IdentityProps, ClassNameProps {
    name: 'save' | 'loader';
}

export default function Icon(props: IconProps) {
    return (
        <DynamicIcon
            id={props.id}
            key={props.key}
            className={props.className ?? 'w-4 h-4'}
            name={props.name}
        />
    );
}
import { Fieldset as PrimeFieldset } from 'primereact/fieldset';
import { BaseHtmlProps, BaseIdentityProps, ChildrenProps } from '../../../props/BaseProps';
import { useLocale } from '../../../hooks/locale/LocaleContext';

interface FieldsetProps extends BaseIdentityProps, BaseHtmlProps, ChildrenProps {
    title: string;
};

export const Fieldset = (props: FieldsetProps) => {
    const {translate} = useLocale();

    return (
        <PrimeFieldset
            id={props.id}
            key={props.key}
            legend={translate(props.title)} 
            className={props.className}
        >
            {props.children}
        </PrimeFieldset>
    );
};
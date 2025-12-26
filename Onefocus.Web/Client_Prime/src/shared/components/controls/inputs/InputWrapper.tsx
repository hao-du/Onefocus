import { useLocale } from '../../../hooks';
import { InputWrapperProps } from '../../props';

const InputWrapper = (props: InputWrapperProps) => {
    const {translate} = useLocale();
    return (
        <div className="flex flex-column gap-2 mb-3">
            {props.label && (
                <label className={`${props.errorMessage ? 'p-error' : ''} ${props.size == 'small' ? 'text-sm font-semibold' : 'font-medium'}`} htmlFor={props.htmlFor}>
                    {translate(props.label)}
                </label>
            )}
            {props.children}
            {props.description && <small className="of-text-200">{translate(props.description)}</small>}
            {props.errorMessage && <small className="p-error">{translate(props.errorMessage)}</small>}
        </div>
    );
};

export default InputWrapper;
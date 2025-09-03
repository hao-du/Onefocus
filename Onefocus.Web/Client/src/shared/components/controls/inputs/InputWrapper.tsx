import { InputWrapperProps } from '../../props';

const InputWrapper = (props: InputWrapperProps) => {
    return (
        <div className="flex flex-column gap-2 mb-3">
            {props.label && (
                <label className={props.errorMessage ? 'p-error' : ''} htmlFor={props.htmlFor}>
                    {props.label}
                </label>
            )}
            {props.children}
            {props.description && <small className="of-text-200">{props.description}</small>}
            {props.errorMessage && <small className="p-error">{props.errorMessage}</small>}
        </div>
    );
};

export default InputWrapper;
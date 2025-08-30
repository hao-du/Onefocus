import { Password as PiPassword } from 'primereact/password';
import { InputProps } from '../../props';

export type PasswordProps = InputProps & {
    floatClassName?: string;
    feedback?: boolean;
    onValueChange?: (value: string | null) => void;
};

const Password = (props: PasswordProps) => {
    return (
        <PiPassword
            inputClassName={props.className}
            inputId={props.id}
            feedback={props.feedback ?? false}
            value={props.value}
            onChange={(e) => {
                if (props.onValueChange) props.onValueChange(e.target.value);
            }}
            readOnly={props.isPending || props.readOnly}
            invalid={props.invalid}
            autoComplete="current-password"
        />
    );
};

export default Password;
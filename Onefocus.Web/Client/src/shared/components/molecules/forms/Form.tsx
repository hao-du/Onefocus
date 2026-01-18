import { Form as AntForm } from "antd";
import { ChildrenProps, ClassNameProps } from "../../../props/BaseProps";

interface FormProps extends ChildrenProps, ClassNameProps {
    onSubmit?: () => void;
    layout?: 'horizontal' | 'inline' | 'vertical';
}

const Form = (props: FormProps) => {
    return (
        <AntForm
            className={props.className}
            onFinish={props.onSubmit}
            layout={props.layout ?? 'vertical'}
        >
            {props.children}
        </AntForm>
    );
};

export default Form;
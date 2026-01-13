import { Form as AntForm } from "antd";
import { ChildrenProps } from "../../../props/BaseProps";

interface FormProps extends ChildrenProps {
    onSubmit?: () => void;
    layout?: 'horizontal' | 'inline' | 'vertical';
}

const Form = (props: FormProps) => {
    return (
        <AntForm
            onFinish={props.onSubmit}
            layout={props.layout ?? 'vertical'}
        >
            {props.children}
        </AntForm>
    );
};

export default Form;
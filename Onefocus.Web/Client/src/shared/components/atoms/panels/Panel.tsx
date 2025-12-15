import { ReactNode } from "react";
import { Panel as PrimePanel } from "primereact/panel";
import { BaseHtmlProps, BaseIdentityProps, ChildrenProps } from "../../../props/BaseProps";

interface PanelHeaderTemplateOptions {
    className?: string;
};

interface PanelProps extends ChildrenProps, BaseIdentityProps, BaseHtmlProps {
    header?: React.ReactNode;
    headerTemplate?: ReactNode | ((options: PanelHeaderTemplateOptions) => React.ReactNode);
    footerTemplate?: ReactNode;
    toggleable?: boolean;
};

export const Panel = (props: PanelProps) => {
    return (
        <PrimePanel
            id={props.id}
            key={props.key}
            header={props.header}
            headerTemplate={props.headerTemplate}
            footerTemplate={props. footerTemplate}
            toggleable={props.toggleable}
            className={props.className}
        >
            {props.children}
        </PrimePanel>
    );
};
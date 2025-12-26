import { PanelHeaderTemplateOptions, Panel as PrimePanel } from "primereact/panel";
import { PropsWithChildren, ReactNode } from "react";
import { BaseProps } from "../props";

type PanelProps = BaseProps & PropsWithChildren & {
    header?: React.ReactNode;
    headerTemplate?: ReactNode | ((options: PanelHeaderTemplateOptions) => React.ReactNode);
    footerTemplate?: ReactNode;
    toggleable?: boolean;
};

const Panel = (props: PanelProps) => {
    return (
        <PrimePanel
            header={props.header}
            headerTemplate={props.headerTemplate}
            footerTemplate={props. footerTemplate}
            toggleable={props.toggleable}
            className={props.className}
        >
            {props.children}
        </PrimePanel>
    );
}

export default Panel;
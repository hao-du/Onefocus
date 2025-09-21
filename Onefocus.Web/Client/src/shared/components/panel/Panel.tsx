import { PanelHeaderTemplateOptions, Panel as PrimePanel } from "primereact/panel";
import { PropsWithChildren, ReactNode } from "react";

type PanelProps = PropsWithChildren & {
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
        >
            {props.children}
        </PrimePanel>
    );
}

export default Panel;
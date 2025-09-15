import { DataView as PrimeDataView } from "primereact/dataview";
import { BaseProps } from "../../props";

type DataViewProps = BaseProps & {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    value?: any[];
    children: React.ReactNode;
    isPending?: boolean;
    dataKey?: string;
    style?: React.CSSProperties;
    header?: React.ReactNode | undefined;
    layout?: 'list' | 'grid' | (string & Record<string, unknown>);
     // eslint-disable-next-line @typescript-eslint/no-explicit-any
     itemTemplate?(item: any, layout?: 'list' | 'grid' | (string & Record<string, unknown>)): React.ReactNode | undefined;
}

const DataView = (props : DataViewProps) => {
    return (
        <PrimeDataView
            value={props.value}
            className={props.className}
            layout={props.layout}
            style={props.style}
            loading={props.isPending}
            loadingIcon="pi pi-spinner pi-spin"
            dataKey={props.dataKey}
            header={props.header}
            itemTemplate={props.itemTemplate}
        >
            {props.children}
        </PrimeDataView>
    );
};

export default DataView;
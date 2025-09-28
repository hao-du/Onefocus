/* eslint-disable @typescript-eslint/no-explicit-any */
import { DataView as PrimeDataView } from "primereact/dataview";
import { BaseProps } from "../../props";

type DataViewProps = BaseProps & {
    value?: any[];
    isPending?: boolean;
    dataKey?: string;
    style?: React.CSSProperties;
    header?: React.ReactNode | undefined;
    layout?: 'list' | 'grid' | (string & Record<string, unknown>);
    emptyMessage?:string;
    itemTemplate?(item: any, layout?: 'list' | 'grid' | (string & Record<string, unknown>)): React.ReactNode | undefined;
    listTemplate?(items: any[], layout?: 'list' | 'grid' | (string & Record<string, unknown>)): React.ReactNode | React.ReactNode[] | undefined;
}

const DataView = (props: DataViewProps) => {
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
            listTemplate={props.listTemplate}
            emptyMessage={props.emptyMessage}
        />
    );
};

export default DataView;
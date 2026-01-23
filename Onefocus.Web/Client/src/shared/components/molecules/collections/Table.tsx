import { Table as AntTable } from "antd";
import { ChildrenProps } from "../../../props/BaseProps";
import { ColumnOption } from "../../../options/ColumnOption";
import Loading from "../../atoms/misc/Loading";

interface TableProps<T> extends ChildrenProps {
    dataSource?: T[],
    columns?: ColumnOption<T>[],
    isPending?: boolean,
}

const Table = <T,>(props: TableProps<T>) => {
    return (
        <AntTable<T>
            dataSource={props.dataSource}
            columns={props.columns}
            pagination={false}
            loading={{
                spinning: props.isPending,
                indicator: <Loading size="xlarge" />
            }}
        >
            {props.children}
        </AntTable>
    );
};

export default Table;
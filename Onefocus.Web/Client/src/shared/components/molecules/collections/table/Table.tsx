import { Table as AntTable } from "antd";
import { ChildrenProps } from "../../../../props/BaseProps";
import { ColumnOption } from "../../../../options/ColumnOption";

interface TableProps<T> extends ChildrenProps {
    dataSource?: T[],
    columns?: ColumnOption<T>[]
}

const Table = <T,>(props: TableProps<T>) => {
    return (
        <AntTable<T>
            dataSource={props.dataSource}
            columns={props.columns}
            pagination={{ placement: ["bottomStart"] }}
            scroll={{ y: 'calc(100vh - 270px)' }}
        >
            {props.children}
        </AntTable>
    );
};

export default Table;
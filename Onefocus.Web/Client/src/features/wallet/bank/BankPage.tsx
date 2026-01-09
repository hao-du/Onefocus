import { useMemo } from "react";
import Icon from "../../../shared/components/atoms/misc/Icon";
import DefaultLayout from "../../../shared/components/layouts/DefaultLayout";
import Table from "../../../shared/components/molecules/collections/table/Table";
import Card from "../../../shared/components/molecules/panels/Card";
import { ActionOption } from "../../../shared/options/ActionOption";
import useGetAllBanks from "./services/useGetAllBanks";

const BankPage = () => {
    const actions: ActionOption[] = useMemo(() => [
        {
            id: 'btnFilter',
            icon: <Icon name="filter" />,
            label: 'Filter',
            command: () => {
            },
        },
        {
            id: 'btnAdd',
            icon: <Icon name="add" />,
            label: 'Add',
            command: () => {
            },
        },
    ], []);

    const { entities, isListLoading, refetch } = useGetAllBanks();

    return (
        <DefaultLayout
            title="Bank List"
            showPrimaryButton
            actions={actions}
            singleCard
        >
            <Card
                className="h-full"
                bodyStyle={{ padding: 0 }}
                body={
                    <Table
                        dataSource={entities}
                        columns={[
                            {
                                key: 'name',
                                title: 'Bank Name',
                                dataIndex: 'name',
                            },
                            {
                                key: 'description',
                                title: 'Description',
                                dataIndex: 'description',
                            }
                        ]}
                    />
                }
            />
        </DefaultLayout>
    );
}

export default BankPage;

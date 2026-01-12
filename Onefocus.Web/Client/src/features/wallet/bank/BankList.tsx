import { useEffect, useMemo } from "react";
import Icon from "../../../shared/components/atoms/misc/Icon";
import DefaultLayout from "../../../shared/components/layouts/DefaultLayout";
import Table from "../../../shared/components/molecules/collections/table/Table";
import Card from "../../../shared/components/molecules/panels/Card";
import { ActionOption } from "../../../shared/options/ActionOption";
import useGetBanks from "./services/useGetBanks";
import usePage from "../../../shared/hooks/page/usePage";
import BankDetail from "./BankDetail";
import BankFilter from "./BankFilter";

const BankList = () => {
    const { filter, setCurrentComponentId, registerRefreshCallback, isPageLoading, setPageLoading } = usePage();
    const { entities, isListLoading, refetch } = useGetBanks(filter);

    const actions = useMemo<ActionOption[]>(() => [
        {
            id: 'btnFilter',
            icon: <Icon name="filter" />,
            label: 'Filter',
            isPending: isPageLoading(),
            command: () => {
                setCurrentComponentId(BankFilter.id);
            },
        },
        {
            id: 'btnAdd',
            icon: <Icon name="add" />,
            label: 'Add',
            isPending: isPageLoading(),
            command: () => {
                setCurrentComponentId(BankDetail.id);
            },
        },
    ], [isPageLoading, setCurrentComponentId]);

    useEffect(() => {
        registerRefreshCallback(refetch);
    }, [refetch, registerRefreshCallback])

    useEffect(() => {
        setPageLoading(isListLoading);
    }, [isListLoading, setPageLoading]);

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
                        isPending={isListLoading}
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

export default BankList;

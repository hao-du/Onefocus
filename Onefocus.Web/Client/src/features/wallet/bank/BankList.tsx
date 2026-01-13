import { useEffect, useMemo } from "react";
import Icon from "../../../shared/components/atoms/misc/Icon";
import DefaultLayout from "../../../shared/components/layouts/DefaultLayout";
import Table from "../../../shared/components/molecules/collections/table/Table";
import Card from "../../../shared/components/molecules/panels/Card";
import { ActionOption } from "../../../shared/options/ActionOption";
import useGetBanks from "./services/useGetBanks";
import usePage from "../../../shared/hooks/page/usePage";
import { BANK_COMPONENT_NAMES } from "../../constants";

const BankList = () => {
    const { filter, openComponent, registerRefreshCallback, hasAnyLoading, setLoadings } = usePage();
    const { entities, isListLoading, refetch } = useGetBanks(filter);

    const actions = useMemo<ActionOption[]>(() => [
        {
            id: 'btnFilter',
            icon: <Icon name="filter" />,
            label: 'Filter',
            isPending: hasAnyLoading,
            command: () => {
                openComponent(BANK_COMPONENT_NAMES.BankFilter);
            },
        },
        {
            id: 'btnAdd',
            icon: <Icon name="add" />,
            label: 'Add',
            isPending: hasAnyLoading,
            command: () => {
                openComponent(BANK_COMPONENT_NAMES.BankDetail);
            },
        },
    ], [hasAnyLoading, openComponent]);

    useEffect(() => {
        registerRefreshCallback(refetch);
    }, [refetch, registerRefreshCallback])

    useEffect(() => {
        setLoadings({
            isListLoading,
        });
    }, [isListLoading, setLoadings]);

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
                        isPending={hasAnyLoading}
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

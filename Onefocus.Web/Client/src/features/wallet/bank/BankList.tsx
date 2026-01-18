import { useEffect, useMemo } from "react";
import Icon from "../../../shared/components/atoms/misc/Icon";
import DefaultLayout from "../../../shared/components/layouts/DefaultLayout";
import Table from "../../../shared/components/molecules/collections/Table";
import Card from "../../../shared/components/molecules/panels/Card";
import { ActionOption } from "../../../shared/options/ActionOption";
import useGetBanks from "./services/useGetBanks";
import usePage from "../../../shared/hooks/page/usePage";
import { BANK_COMPONENT_NAMES } from "../../constants";
import Button from "../../../shared/components/atoms/buttons/Button";

const BankList = () => {
    const { filter, openComponent, registerRefreshCallback, setDataId, hasAnyLoading, setLoadings } = usePage();
    const { banks, isBanksLoading, refetch } = useGetBanks(filter ?? {});

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
            isBanksLoading,
        });
    }, [isBanksLoading, setLoadings]);

    return (
        <DefaultLayout
            title="Bank List"
            showPrimaryButton
            actions={actions}
        >
            <Card
                className="h-full"
                bodyStyle={{ padding: 0 }}
                body={
                    <Table
                        dataSource={banks}
                        isPending={hasAnyLoading}
                        columns={[
                            {
                                key: 'name',
                                title: 'Bank Name',
                                dataIndex: 'name',
                                render: (_, record) => {
                                    return (
                                        <Button
                                            variant="link"
                                            text={record.name}
                                            onClick={() => {
                                                setDataId(record.id);
                                                openComponent(BANK_COMPONENT_NAMES.BankDetail);
                                            }}
                                        />
                                    );
                                }
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

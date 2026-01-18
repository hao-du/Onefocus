import { useEffect, useMemo } from "react";
import Icon from "../../../shared/components/atoms/misc/Icon";
import DefaultLayout from "../../../shared/components/layouts/DefaultLayout";
import Table from "../../../shared/components/molecules/collections/Table";
import Card from "../../../shared/components/molecules/panels/Card";
import { ActionOption } from "../../../shared/options/ActionOption";
import usePage from "../../../shared/hooks/page/usePage";
import { CURRENCY_COMPONENT_NAMES } from "../../constants";
import Button from "../../../shared/components/atoms/buttons/Button";
import useGetAllCurrencies from "./services/useGetAllCurrencies";
import Switch from "../../../shared/components/atoms/inputs/Switch";

const CurrencyList = () => {
    const { openComponent, registerRefreshCallback, setDataId, hasAnyLoading, setLoadings } = usePage();
    const { currencies: entities, isCurrenciesLoading: isListLoading, refetch } = useGetAllCurrencies();

    const actions = useMemo<ActionOption[]>(() => [
        {
            id: 'btnAdd',
            icon: <Icon name="add" />,
            label: 'Add',
            isPending: hasAnyLoading,
            command: () => {
                openComponent(CURRENCY_COMPONENT_NAMES.CurrencyDetail);
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
            title="Counterparty List"
            showPrimaryButton
            actions={actions}
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
                                title: 'Name',
                                dataIndex: 'name',
                                render: (_, record) => {
                                    return (
                                        <Button
                                            variant="link"
                                            text={record.name}
                                            onClick={() => {
                                                setDataId(record.id);
                                                openComponent(CURRENCY_COMPONENT_NAMES.CurrencyDetail);
                                            }}
                                        />
                                    );
                                }
                            },
                            {
                                key: 'shortName',
                                title: 'Short Name',
                                dataIndex: 'shortName',
                            },
                            {
                                key: 'isDefault',
                                title: 'Default',
                                dataIndex: 'isDefault',
                                render: (_, record) => {
                                    if (!record.isDefault) return undefined;
                                    return <Switch key={record.id} value={true} disabled small />;
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

export default CurrencyList;

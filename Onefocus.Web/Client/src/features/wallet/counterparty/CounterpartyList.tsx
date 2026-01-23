import { useEffect, useMemo } from "react";
import Icon from "../../../shared/components/atoms/misc/Icon";
import DefaultLayout from "../../../shared/components/layouts/DefaultLayout";
import Table from "../../../shared/components/molecules/collections/Table";
import Card from "../../../shared/components/molecules/panels/Card";
import { ActionOption } from "../../../shared/options/ActionOption";
import usePage from "../../../shared/hooks/page/usePage";
import { COUNTERPARTY_COMPONENT_NAMES } from "../../constants";
import Button from "../../../shared/components/atoms/buttons/Button";
import useGetAllCounterparties from "./services/useGetAllCounterparties";

const CounterpartyList = () => {
    const { openComponent, setDataId, hasAnyLoading, setLoadings } = usePage();
    const { counterparties, isCounterpartiesLoading } = useGetAllCounterparties();

    const actions = useMemo<ActionOption[]>(() => [
        {
            id: 'btnAdd',
            icon: <Icon name="add" />,
            label: 'Add',
            isPending: hasAnyLoading,
            command: () => {
                openComponent(COUNTERPARTY_COMPONENT_NAMES.CounterpartyDetail);
            },
        },
    ], [hasAnyLoading, openComponent]);

    useEffect(() => {
        setLoadings({
            isCounterpartiesLoading,
        });
    }, [isCounterpartiesLoading, setLoadings]);

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
                        dataSource={counterparties}
                        isPending={hasAnyLoading}
                        columns={[
                            {
                                key: 'fullName',
                                title: 'Full Name',
                                dataIndex: 'fullName',
                                render: (_, record) => {
                                    return (
                                        <Button
                                            variant="link"
                                            text={record.fullName}
                                            onClick={() => {
                                                setDataId(record.id);
                                                openComponent(COUNTERPARTY_COMPONENT_NAMES.CounterpartyDetail);
                                            }}
                                        />
                                    );
                                }
                            },
                            {
                                key: 'email',
                                title: 'Email',
                                dataIndex: 'email',
                            },
                            {
                                key: 'phoneNumber',
                                title: 'Phone',
                                dataIndex: 'phoneNumber',
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

export default CounterpartyList;

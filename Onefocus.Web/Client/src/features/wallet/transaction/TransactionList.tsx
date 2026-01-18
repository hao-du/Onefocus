import { useEffect, useMemo } from "react";
import Icon from "../../../shared/components/atoms/misc/Icon";
import DefaultLayout from "../../../shared/components/layouts/DefaultLayout";
import Card from "../../../shared/components/molecules/panels/Card";
import { ActionOption } from "../../../shared/options/ActionOption";
import usePage from "../../../shared/hooks/page/usePage";
import { TRANSACTION_COMPONENT_NAMES } from "../../constants";
import useGetAllTransactions from "./services/useGetAllTransactions";
import Repeater from "../../../shared/components/molecules/collections/Repeater";
import { formatCurrency } from "../../../shared/utils";
import useLocale from "../../../shared/hooks/locale/useLocale";
import CardTitle from "../../../shared/components/atoms/typography/CardTitle";
import ExtraInfo from "../../../shared/components/atoms/typography/ExtraInfo";
import Text from "../../../shared/components/atoms/typography/Text";
import Tag from "../../../shared/components/atoms/misc/Tag";
import Button from "../../../shared/components/atoms/buttons/Button";
import Space from "../../../shared/components/atoms/panels/Space";
import { getComponentName } from "./shared";

const TransactionList = () => {
    const { openComponent, registerRefreshCallback, setDataId, hasAnyLoading, setLoadings } = usePage();
    const { formatDateTime } = useLocale();
    const { transactions, isTransactionsLoading, refetch } = useGetAllTransactions();

    const actions = useMemo<ActionOption[]>(() => [
        {
            id: 'btnFilter',
            icon: <Icon name="filter" />,
            label: 'Filter',
            isPending: hasAnyLoading,
            command: () => {
                openComponent(TRANSACTION_COMPONENT_NAMES.TransactionFilter);
            },
        },
        {
            id: 'btnAddCashFlow',
            icon: <Icon name="cashFlow" />,
            label: 'Cashflow',
            isPending: hasAnyLoading,
            command: () => {
                openComponent(TRANSACTION_COMPONENT_NAMES.CashFlow);
            },
        },
        {
            id: 'btnAddBankAccount',
            icon: <Icon name="bankAccount" />,
            label: 'Bank Account',
            isPending: hasAnyLoading,
            command: () => {
                openComponent(TRANSACTION_COMPONENT_NAMES.BankAccount);
            },
        },
        {
            id: 'btnAddPeerTransfer',
            icon: <Icon name="peetTransfer" />,
            label: 'Peer Transfer',
            isPending: hasAnyLoading,
            command: () => {
                openComponent(TRANSACTION_COMPONENT_NAMES.PeerTransfer);
            },
        },
        {
            id: 'btnAddCurrencyExchange',
            icon: <Icon name="currencyExchange" />,
            label: 'Currency Exchange',
            isPending: hasAnyLoading,
            command: () => {
                openComponent(TRANSACTION_COMPONENT_NAMES.CurrencyExchange);
            },
        }
    ], [hasAnyLoading, openComponent]);

    useEffect(() => {
        registerRefreshCallback(refetch);
    }, [refetch, registerRefreshCallback])

    useEffect(() => {
        setLoadings({
            isTransactionsLoading,
        });
    }, [isTransactionsLoading, setLoadings]);

    return (
        <DefaultLayout
            title="Transactions"
            showPrimaryButton
            actions={actions}
        >
            <div className="w-full flex flex-row gap-4 flex-wrap lg:flex-nowrap">
                <div className="basis-full lg:basis-2/3 lg:order-2">
                    <div className="sticky top-0">
                        <Card
                            title="Statistic Sections"
                            body={<>(To be implemented...)</>}
                        />
                    </div>
                </div>
                <div className="basis-full lg:basis-1/3 lg:order-1">
                    <Card
                        bodyStyle={{ padding: 0 }}
                        body={
                            <Repeater
                                dataSource={transactions}
                                body={(record) => {
                                    const meta = getComponentName(record.type);
                                    if (!meta) return null;

                                    return (
                                        <div className="p-4 grid gap-2 grid-cols-6 border-t border-(--ant-color-border-secondary)">
                                            <div className="col-span-3">
                                                <Space size="small">
                                                    <CardTitle title={formatDateTime(record.transactedOn, false)} />
                                                    <Button
                                                        variant="link"
                                                        color="primary"
                                                        size="small"
                                                        icon={<Icon name="edit" size="small" />}
                                                        onClick={() => {
                                                            setDataId(record.id);
                                                            openComponent(meta.componentName);
                                                        }}
                                                    />
                                                </Space>
                                            </div>
                                            <div className="col-span-3 inline-flex justify-end">
                                                <Text text={formatCurrency(record.amount)} strong />
                                                <ExtraInfo text={record.currencyName ?? ''} className="w-10" align="right" />
                                            </div>
                                            <div className="col-span-6">
                                                <ExtraInfo text={record.description ?? ''} align="justify" />
                                            </div>
                                            <div className="col-span-6 inline-flex flex-wrap gap-2">
                                                {record.tags?.map((value, index) => {
                                                    return <Tag key={index} value={value} color={meta.color} />
                                                })}
                                            </div>
                                        </div>
                                    );
                                }}
                            />
                        }
                    />
                </div>
            </div>

        </DefaultLayout>
    );
}

export default TransactionList;

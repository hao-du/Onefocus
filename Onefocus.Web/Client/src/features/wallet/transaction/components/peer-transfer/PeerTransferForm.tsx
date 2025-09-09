import { UniqueComponentId } from 'primereact/utils';
import { useMemo } from 'react';
import { useForm } from 'react-hook-form';
import { Option } from '../../../../../shared/components/controls';
import { Column, EditableTable } from '../../../../../shared/components/data';
import { DatePicker, Dropdown, Number, Switch, Text, Textarea } from '../../../../../shared/components/controls';
import { WorkspaceRightPanel } from '../../../../../shared/components/layouts/workspace';
import { getEmptyGuid } from '../../../../../shared/utils/formatUtils';
import { CurrencyResponse } from '../../../currency';
import PeerTransferFormInput from './interfaces/PeerTransferFormInput';
import { CounterpartyResponse } from '../../../counterparty/apis';

type PeerTransferFormProps = {
    selectedPeerTransfer: PeerTransferFormInput | null | undefined;
    onSubmit: (data: PeerTransferFormInput) => void;
    isPending?: boolean;
    currencies: CurrencyResponse[]
    counterparties: CounterpartyResponse[]
}

const PeerTransferForm = (props: PeerTransferFormProps) => {
    const formValues = useMemo(() => {
        return props.selectedPeerTransfer ??
            {
                id: undefined,
                status: 0,
                type: 0,
                counterpartyId: getEmptyGuid(),
                description: undefined,
                isActive: true,
                transferTransactions: [],
            }
    }, [props.selectedPeerTransfer]);

    const form = useForm<PeerTransferFormInput>({
        defaultValues: formValues,
        values: formValues
    });

    const isEditMode = Boolean(props.selectedPeerTransfer);

    const currencyDropdownOptions = useMemo((): Option[] => {
        return props.currencies.map((currency) => {
            return {
                label: `${currency.shortName} - ${currency.name}`,
                value: currency.id
            } as Option;
        });
    }, [props.currencies])

    const counterpartyDropdownOptions = useMemo((): Option[] => {
        return props.counterparties.map((counterparty) => {
            return {
                label: counterparty.fullName,
                value: counterparty.id
            } as Option;
        });
    }, [props.counterparties])

    const buttons = [
        {
            id: 'btnSave',
            label: 'Save',
            icon: 'pi pi-save',
            onClick: () => {
                form.handleSubmit(props.onSubmit)();
            }
        }
    ];

    return (
        <WorkspaceRightPanel buttons={buttons} isPending={props.isPending}>
            <h3 className="mt-0 mb-5">{`${isEditMode ? 'Edit' : 'Add'} Bank Account`}</h3>
            <form key={props.selectedPeerTransfer?.id ?? 'new'}>
                <Dropdown control={form.control} name="type" label="Type" className="w-full of-w-max" options={[
                    { label: 'Lent to', value: 100 },
                    { label: 'Borrowed from', value: 200 },
                    { label: 'Gave to', value: 300 },
                    { label: 'Received from', value: 400 }
                ]} rules={{
                    validate: { 
                        required: (value) => value && value != 0 ? true : 'Type is required.'
                    }
                }} filter={false}/>
                <Dropdown control={form.control} name="counterpartyId" label="Counterparty" className="w-full of-w-max" options={counterpartyDropdownOptions} rules={{
                    validate: {
                        required: (value) => value && value !== getEmptyGuid() ? true : 'Counterparty is required.'
                    }
                }} />
                <Dropdown control={form.control} name="status" label="Status" className="w-full of-w-max" options={[
                    { label: 'Completed', value: 100 },
                    { label: 'Failed', value: 200 },
                    { label: 'Processing', value: 300 },
                    { label: 'Pending', value: 400 }
                ]} rules={{
                    validate: {
                        required: (value) => value && value != 0 ? true : 'Status is required.'
                    }
                }} filter={false}/>
                <Textarea control={form.control} name="description" label="Description" className="w-full of-w-max" rules={{
                    maxLength: { value: 255, message: 'Name cannot exceed 255 characters.' },
                }} />
                {isEditMode && <Switch control={form.control} name="isActive" label="Is active" />}

                <EditableTable
                    isPending={props.isPending}
                    form={form}
                    path='transferTransactions'
                    newRowValue={{
                        rowId: UniqueComponentId(),
                        transactedOn: new Date(),
                        amount: 0,
                        currencyId: getEmptyGuid(),
                        description: '',
                        isInFlow: undefined,
                        isActive: true
                    }}
                    tableName='Interests'
                    style={{ width: '40rem' }}
                >
                    <Column field="transactedOn" header="Transacted On" style={{ width: '12rem' }} editor={(options) => {
                        return (
                            <DatePicker
                                name={`transferTransactions.${options.rowIndex}.transactedOn`}
                                control={form.control}
                                className="w-full"
                                rules={{
                                    required: 'Transacted On is required.'
                                }}
                                appendTo={document.body}
                            />);
                    }} />
                    <Column field="amount" header="Amount" style={{ width: '13rem' }} align="right" editor={(options) => {
                        return (
                            <Number
                                control={form.control}
                                name={`transferTransactions.${options.rowIndex}.amount`}
                                inputClassName="w-full"
                                fractionDigits={2}
                                rules={{
                                    required: 'Amount is required.',
                                    min: { value: 0, message: "Minimum amount is 0" },
                                }}
                            />);
                    }} />
                    <Column field="currencyId" header="Currency" style={{ width: '13rem' }} align="right" editor={(options) => {
                        return (
                            <Dropdown
                                control={form.control}
                                name={`transferTransactions.${options.rowIndex}.currencyId`}
                                options={currencyDropdownOptions}
                                className="w-full"
                                rules={{
                                    validate: {
                                        required: (value) => value && value !== getEmptyGuid() ? true : 'Currency is required.'
                                    }
                                }}
                            />);
                    }} />
                    <Column field="isInFlow" header="Cash flow" style={{ width: '10rem' }} align="right" editor={(options) => {
                        return (
                            <Dropdown
                                control={form.control}
                                name={`transferTransactions.${options.rowIndex}.isInFlow`}
                                options={[ { label: 'Inflow', value: true }, { label: 'Outflow', value: false } ]}
                                className="w-full"
                                filter={false}
                                rules={{
                                    validate: {
                                        required: (value) => value !== undefined ? true : 'Cash flow is required.'
                                    }
                                }}
                            />);
                    }} />
                    <Column field="description" style={{ width: '15rem' }} header="Description" editor={(options) => {
                        return (
                            <Text
                                control={form.control}
                                name={`transferTransactions.${options.rowIndex}.description`}
                                className="w-full"
                            />);
                    }} />
                </EditableTable>
            </form>
        </WorkspaceRightPanel>
    );
};

export default PeerTransferForm;
import {useForm} from 'react-hook-form';
import {useWorkspace, Workspace} from '../../layouts/workspace';
import useGetAllCurrencies from '../../../application/currency/useGetAllCurrencies';
import {Button, SplitButtonActionItem} from '../../components/controls/buttons';
import {Text} from '../../components/form-controls';
import {Currency as DomainCurrency } from '../../../domain/currency';
import {Column, DataTable} from '../../components/data';


interface IFormInput {
    id?: string
    name: string;
    shortName: string;
    isDefault: boolean;
    description?: string;
}

export const Currency = () => {
    const {viewRightPanel, setViewRightPanel} = useWorkspace();
    const {data} = useGetAllCurrencies();

    const {control, reset, getValues } = useForm<IFormInput>({
        defaultValues: undefined
    });

    const actionItems: SplitButtonActionItem[] = [
        {label: 'Add', icon: 'pi pi-plus', command: () => { reset({ name: 'newname' }); }},
    ];

    const hasCurrency = () => {
        const value = getValues();
        return value?.name;
    }

    return (
        <Workspace
            title="Currency"
            actionItems={actionItems}
            leftPanel={
                <div className="overflow-auto flex-1">
                    <DataTable value={data} className="p-datatable-sm">
                        <Column field="name" header="Name"/>
                        <Column field="shortName" header="Short Name"/>
                        <Column field="description" header="Description"/>
                        <Column body={(currency: DomainCurrency) => (
                            <Button
                                icon="pi pi-pencil"
                                className="p-button-text"
                                onClick={() => {
                                    setViewRightPanel(true);
                                    reset(currency);
                                }}
                            />
                        )} header="" headerStyle={{width: "4rem"}}/>
                    </DataTable>
                </div>
            }
            rightPanelProps={{
                viewRightPanel,
                setViewRightPanel,
                buttons: hasCurrency() ? [
                    {
                        onClick: () => {
                        },
                        id: 'btnSave',
                        label: 'Save',
                        icon: 'pi pi-save'
                    }
                ] : []
            }}
            rightPanel={
                hasCurrency() &&
                <>
                    <h3 className="mt-0 mb-5">Edit User</h3>
                    <form>
                        <Text
                            control={control}
                            name="name"
                            label="Name"
                        />
                        <Text
                            control={control}
                            name="shortName"
                            label="Short Name"
                        />
                        <Text
                            control={control}
                            name="description"
                            label="Description"
                        />
                    </form>
                </>
            }
        />
    );
};
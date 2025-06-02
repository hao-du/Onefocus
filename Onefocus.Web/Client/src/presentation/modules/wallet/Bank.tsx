import { useForm } from 'react-hook-form';
import { useWorkspace, Workspace } from '../../layouts/workspace';
import { useGetAllBanks, useGetBankById, useCreateBank, useUpdateBank } from '../../../application/bank';
import { Button, SplitButtonActionItem } from '../../components/controls/buttons';
import { Text, Textarea, Switch } from '../../components/form-controls';
import { Bank as DomainBank } from '../../../domain/bank';
import { Column, DataTable } from '../../components/data';
import { IFormInputProps } from '../../props/FormInputProps';

interface IFormInput extends IFormInputProps {
    id?: string
    name: string;
    isActive: boolean;
    description?: string;
}

export const Bank = () => {
    const { banks } = useGetAllBanks();
    const { onCreateAsync, isCreating } = useCreateBank();
    const { onUpdateAsync, isUpdating } = useUpdateBank();

    const { control, reset, watch, handleSubmit } = useForm<IFormInput>({
        defaultValues: { showForm: false, isNew: true }
    });
    const watchShowForm = watch('showForm');
    const watchIsNewField = watch('isNew');

    const onSubmit = async (data: IFormInput) => {
        if (!data.id) {
            onCreateAsync({
                name: data.name,
                description: data.description
            });
        }
        else {
            onUpdateAsync({
                id: data.id,
                name: data.name,
                isActive: data.isActive,
                description: data.description
            });
        }
    };

    const { viewRightPanel, setViewRightPanel } = useWorkspace();
    return (
        <Workspace
            title="Bank"
            actionItems={[
                {
                    label: 'Add',
                    icon: 'pi pi-plus',
                    command: () => {
                        reset({ showForm: true, isNew: true });
                        setViewRightPanel(true);
                    }
                }
            ]}
            leftPanel={
                <div className="overflow-auto flex-1">
                    <DataTable value={banks} className="p-datatable-sm">
                        <Column field="name" header="Name" />
                        <Column field="shortName" header="Short Name" />
                        <Column field="description" header="Description" />
                        <Column body={(bank: DomainBank) => (
                            <Button
                                icon="pi pi-pencil"
                                className="p-button-text"
                                onClick={() => {
                                    setViewRightPanel(true);
                                    reset({ ...bank, showForm: true, isNew: false });
                                }}
                            />
                        )} header="" headerStyle={{ width: "4rem" }} />
                    </DataTable>
                </div>
            }
            rightPanel={
                watchShowForm &&
                <>
                    <h3 className="mt-0 mb-5">{`${watchIsNewField ? 'Add' : 'Edit'} Bank`}</h3>
                    <form>
                        <Text control={control} name="name" label="Name" />
                        <Textarea control={control} name="description" label="Description" />
                        {!watchIsNewField && <Switch control={control} name="isActive" label="Is active" />}
                    </form>
                </>
            }
            rightPanelProps={{
                viewRightPanel,
                setViewRightPanel,
                buttons: watchShowForm ? [
                    {
                        onClick: () => {
                            handleSubmit(onSubmit)();
                        },
                        id: 'btnSave',
                        label: 'Save',
                        icon: 'pi pi-save'
                    }
                ] : []
            }}
        />
    );
};
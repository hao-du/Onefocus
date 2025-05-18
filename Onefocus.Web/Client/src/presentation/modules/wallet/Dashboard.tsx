import {User} from '../../../domain/user';
import {useWorkspace, Workspace} from '../../layouts/workspace';
import useGetUser from '../../../application/user/useGetUser';
import {Button, SplitButtonActionItem} from '../../components/controls/buttons';
import {Text} from '../../components/form-controls';
import {useForm} from 'react-hook-form';
import {Column, DataTable} from '../../components/data';

interface IFormInput {
    userName: string,
}

export const Dashboard = () => {

    const {viewRightPanel, setViewRightPanel} = useWorkspace();
    const {data} = useGetUser();

    const actionItems: SplitButtonActionItem[] = [
        {label: 'Export', icon: 'pi pi-upload', command: () => console.log('Export clicked')},
        {label: 'Import', icon: 'pi pi-download', command: () => console.log('Import clicked')},
        {label: 'Delete All', icon: 'pi pi-trash', command: () => console.log('Delete All clicked')}
    ];

    const {control, reset, getValues } = useForm<IFormInput>({
        defaultValues: undefined
    });

    const hasUser = () => {
        const value = getValues();
        return value?.userName;
    }

    return (
        <Workspace
            title="User Management"
            actionItems={actionItems}
            leftPanel={
                <div className="overflow-auto flex-1">
                    <DataTable value={data} className="p-datatable-sm">
                        <Column field="userName" header="User Name"/>
                        <Column field="email" header="Email"/>
                        <Column body={(user: User) => (
                            <Button
                                icon="pi pi-pencil"
                                className="p-button-text"
                                onClick={() => {
                                    setViewRightPanel(true);
                                    reset(user);
                                }}
                            />
                        )} header="" headerStyle={{width: "4rem"}}/>
                    </DataTable>
                </div>
            }
            rightPanelProps={{
                viewRightPanel,
                setViewRightPanel,
                buttons: hasUser() ? [
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
                hasUser() &&
                <>
                    <h3 className="mt-0 mb-5">Edit User</h3>
                    <form>
                        <Text
                            control={control}
                            name="userName"
                            label="Username"
                        />
                    </form>
                </>
            }
        />
    );
};
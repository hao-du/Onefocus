import useGetUser from "../../../application/user/useGetUser";
import {useState} from "react";
import WorkspaceLayout from "../../layouts/workspace-layout/WorkspaceLayout";
import {InputText} from "primereact/inputtext";
import {User} from "../../../domain/user";
import {Column} from "primereact/column";
import {Button} from "primereact/button";
import {DataTable} from "primereact/datatable";
import {useWorkspace} from "../../layouts/workspace-layout/useWorkspace";

const Dashboard = () => {
    const { data } = useGetUser();
    const [selectedUser, setSelectedUser] = useState<User | null>(null);
    const { viewRightPanel, setViewRightPanel } = useWorkspace();

    const actionItems = [
        { label: 'Export', icon: 'pi pi-upload', command: () => console.log('Export clicked') },
        { label: 'Import', icon: 'pi pi-download', command: () => console.log('Import clicked') },
        { label: 'Delete All', icon: 'pi pi-trash', command: () => console.log('Delete All clicked') }
    ];

    return (
        <>
            <WorkspaceLayout
                title="User Management"
                actionItems={actionItems}
                leftPanel={
                    <div className="overflow-auto flex-1">
                        <DataTable value={data} className="p-datatable-sm">
                            <Column field="userName" header="User Name" />
                            <Column field="email" header="Email" />
                            <Column body={(user: User) => (
                                <Button
                                    icon="pi pi-pencil"
                                    className="p-button-text"
                                    onClick={() => {
                                        setSelectedUser(user)
                                        setViewRightPanel(true)
                                    }}
                                />
                            )} header="" headerStyle={{ width: "4rem" }} />
                        </DataTable>
                    </div>
                }
                rightPanelProps={{
                    viewRightPanel,
                    setViewRightPanel,
                    buttons: [
                        {
                            onClick: () => {},
                            id: 'btnSave',
                            label: 'Save',
                            icon : 'pi pi-save'
                        }
                ]}}
                rightPanel={
                    selectedUser &&
                    <>
                        <h3 className="mt-0">Edit User</h3>
                        <label className="block mb-2">User Name</label>
                        <InputText
                            className="w-full"
                            value={selectedUser.userName}
                        />
                    </>
                }
            />
        </>
    );
};

export default Dashboard;
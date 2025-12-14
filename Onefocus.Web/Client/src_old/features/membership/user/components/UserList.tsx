import { Workspace } from '../../../../shared/components/layouts/workspace';
import { useCreateUser, useGetAllUsers, useGetUserById, useSyncUsers, useUpdatePassword, useUpdateUser } from '../services';
import { Column, DataTable } from '../../../../shared/components/data/data-table';
import UserFormInput from './interfaces/UserFormInput';
import UserForm from './UserForm';
import React, { useCallback, useEffect, useState } from 'react';
import { useWindows } from '../../../../shared/components/hooks';
import DropdownButton from '../../../../shared/components/controls/buttons/DropdownButton';
import ChangePasswordForm from './ChangePasswordForm';
import ChangePasswordFormInput from './interfaces/ChangePasswordFormInput';
import { useSearchParams } from 'react-router';
import { UserResponse } from '../apis/interfaces';

const UserList = React.memo(() => {
    const [searchParams, setSearchParams] = useSearchParams();

    const [showForm, setShowForm] = useState(0);
    const { showResponseToast } = useWindows();

    const { entities: users, isListLoading, refetch } = useGetAllUsers();
    const { onCreateAsync, isCreating } = useCreateUser();
    const { onUpdateAsync, isUpdating } = useUpdateUser();
    const { onSyncAsync, isSynching } = useSyncUsers();
    const { entity: selectedUser, isEntityLoading, setUserId } = useGetUserById();
    const { onUpdatePasswordAsync, isUpdatingPassword } = useUpdatePassword();

    const isPending = isListLoading || isEntityLoading || isCreating || isUpdating || isSynching || isUpdatingPassword;

    const onSubmit = useCallback(async (data: UserFormInput) => {
        if (!data.id) {
            const response = await onCreateAsync({
                email: data.email,
                firstName: data.firstName,
                lastName: data.lastName,
                password: data.password ?? ''
            });
            showResponseToast(response, 'Saved successfully.');
            if (response.status === 200 && response.value.id) {
                setUserId(response.value.id);
            }
        }
        else {
            const response = await onUpdateAsync({
                id: data.id,
                firstName: data.firstName,
                lastName: data.lastName,
                email: data.email
            });
            showResponseToast(response, 'Updated successfully.');
        }
        refetch();
    }, [onCreateAsync, onUpdateAsync, refetch, setUserId, showResponseToast]);

    const onSubmitPassword = useCallback(async (data: ChangePasswordFormInput) => {
        const response = await onUpdatePasswordAsync({
            id: data.id,
            password: data.password,
            confirmPassword: data.confirmPassword
        });
        showResponseToast(response, 'Updated password successfully.');
    }, [onUpdatePasswordAsync, showResponseToast]);

    useEffect(() => {
        const id = searchParams.get("id");
        if (id) {
            setUserId(id);
            setShowForm(1);
        }
    }, [searchParams, setUserId]);

    return (
        <Workspace
            title="Users"
            isPending={isPending}
            actionItems={[
                {
                    label: 'Actions...',
                    items: [
                        {
                            label: 'Add',
                            icon: 'pi pi-plus',
                            command: () => {
                                setUserId(undefined);
                                setShowForm(1);
                            }
                        },
                        {
                            label: 'Sync users',
                            icon: 'pi pi-sync',
                            command: async () => {
                                const response = await onSyncAsync();
                                showResponseToast(response, 'Synced successfully.');
                            }
                        },
                    ],
                },
            ]}

            leftPanel={
                <div className="overflow-auto flex-1">
                    <DataTable value={users} isPending={isPending} className="p-datatable-sm">
                        <Column field="email" header="Email" className="w-auto" />
                        <Column field="firstName" header="First name" className="w-3" />
                        <Column field="lastName" header="Last name" className="w-3" />
                        <Column className="w-1rem" body={(user: UserResponse) => (
                            <DropdownButton
                                text
                                rounded
                                actionItems={[
                                    {
                                        label: 'Edit',
                                        icon: 'pi pi-pencil',
                                        command: () => {
                                            setUserId(user.id);
                                            setShowForm(1);
                                            setSearchParams({ id: user.id});
                                        }
                                    },
                                    {
                                        label: 'Change password',
                                        icon: 'pi pi-unlock',
                                        command: async () => {
                                            setUserId(user.id);
                                            setShowForm(2);
                                        }
                                    }
                                ]}
                            />
                        )} header="" headerStyle={{ width: "4rem" }} />
                    </DataTable>
                </div>
            }
            rightPanel={
                <>
                    {showForm == 0 && <></>}
                    {showForm == 1 && <UserForm selectedUser={selectedUser} isPending={isPending} onSubmit={onSubmit} />}
                    {showForm == 2 && <ChangePasswordForm selectedUser={{
                        id: selectedUser?.id ?? '',
                        password: '',
                        confirmPassword: '',
                        fullName: selectedUser ? `${selectedUser.firstName} ${selectedUser.lastName}`.trim() : undefined
                    }} isPending={isPending} onSubmit={onSubmitPassword} />}
                </>
            }
        />
    );
});

export default UserList;
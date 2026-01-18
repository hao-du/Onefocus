import { useEffect, useMemo } from "react";
import Icon from "../../../shared/components/atoms/misc/Icon";
import DefaultLayout from "../../../shared/components/layouts/DefaultLayout";
import Table from "../../../shared/components/molecules/collections/Table";
import Card from "../../../shared/components/molecules/panels/Card";
import { ActionOption } from "../../../shared/options/ActionOption";
import usePage from "../../../shared/hooks/page/usePage";
import { USER_COMPONENT_NAMES } from "../../constants";
import Button from "../../../shared/components/atoms/buttons/Button";
import useGetAllUsers from "./services/useGetAllUsers";
import useWindows from "../../../shared/hooks/windows/useWindows";
import useSyncUsers from "./services/useSyncUsers";
import { getFullName } from "../../../shared/utils";
import { Space } from "antd";

const UserList = () => {
    const { openComponent, registerRefreshCallback, setDataId, hasAnyLoading, setLoadings } = usePage();
    const { users, isUsersLoading, refetch } = useGetAllUsers();
    const { syncAsync, isSynching } = useSyncUsers();
    const { showResponseToast } = useWindows();

    const actions = useMemo<ActionOption[]>(() => [
        {
            id: 'btnAdd',
            icon: <Icon name="add" />,
            label: 'Add',
            isPending: hasAnyLoading,
            command: () => {
                openComponent(USER_COMPONENT_NAMES.UserDetail);
            },
        },
        {
            id: 'btnSync',
            icon: <Icon name="sync" />,
            label: 'Sync',
            isPending: hasAnyLoading,
            command: async () => {
                const response = await syncAsync();
                showResponseToast(response, 'Synced successfully.');
            },
        },
    ], [hasAnyLoading, openComponent, showResponseToast, syncAsync]);

    useEffect(() => {
        registerRefreshCallback(refetch);
    }, [refetch, registerRefreshCallback])

    useEffect(() => {
        setLoadings({
            isUsersLoading, isSynching
        });
    }, [isUsersLoading, isSynching, setLoadings]);

    return (
        <DefaultLayout
            title="User List"
            showPrimaryButton
            actions={actions}
        >
            <Card
                className="h-full"
                bodyStyle={{ padding: 0 }}
                body={
                    <Table
                        dataSource={users}
                        isPending={hasAnyLoading}
                        columns={[
                            {
                                key: 'userName',
                                title: 'User Name',
                                dataIndex: 'userName',
                                render: (_, record) => {
                                    return (
                                        <Space size="small">
                                            <Button
                                                variant="link"
                                                text={getFullName(record.firstName, record.lastName)}
                                                className="px-0!"
                                                onClick={() => {
                                                    setDataId(record.id);
                                                    openComponent(USER_COMPONENT_NAMES.UserDetail);
                                                }}
                                            />
                                            <Button
                                                variant="link"
                                                color="danger"
                                                text="(Change Password)"
                                                className="px-0!"
                                                onClick={() => {
                                                    setDataId(record.id);
                                                    openComponent(USER_COMPONENT_NAMES.UpdatePassword);
                                                }}
                                            />
                                        </Space>
                                    );
                                }
                            },
                            {
                                key: 'email',
                                title: 'Email',
                                dataIndex: 'email',
                            }
                        ]}
                    />
                }
            />
        </DefaultLayout>
    );
}

export default UserList;

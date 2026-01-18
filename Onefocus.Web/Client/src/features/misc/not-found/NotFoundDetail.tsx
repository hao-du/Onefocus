import { useNavigate } from 'react-router';
import Empty from "../../../shared/components/atoms/misc/Empty";
import Icon from "../../../shared/components/atoms/misc/Icon";
import Space from "../../../shared/components/atoms/panels/Space";
import DefaultLayout from "../../../shared/components/layouts/DefaultLayout";
import Card from "../../../shared/components/molecules/panels/Card";
import Button from '../../../shared/components/atoms/buttons/Button';

const NotFoundDetail = () => {
    const navigate = useNavigate();

    return (
        <DefaultLayout
            singleCard
            workspaceClassName="pt-4"
        >
            <Card
                className="h-full"
                bodyStyle={{ padding: 0, height: '100%' }}
                body={
                    <Empty
                        className="h-full flex flex-col justify-center"
                        label="Oops! Nothing here but ghosts."
                        image={(
                            <Space>
                                <Icon name="ghost" size="small"></Icon>
                                <Icon name="ghost" size="middle"></Icon>
                                <Icon name="ghost" size="large"></Icon>
                                <Icon name="ghost" size="xLarge"></Icon>
                                <Icon name="ghost" size="xxLarge"></Icon>
                                <Icon name="ghost" size="xLarge"></Icon>
                                <Icon name="ghost" size="large"></Icon>
                                <Icon name="ghost" size="middle"></Icon>
                                <Icon name="ghost" size="small"></Icon>
                            </Space>
                        )}
                    >
                        <Button
                            variant="dashed"
                            text="Take me home"
                            icon={<Icon name="house" />}
                            onClick={() => navigate('/')}
                        />
                    </Empty>
                }
            />
        </DefaultLayout>
    );
}

export default NotFoundDetail;

import Drawer from "../../../shared/components/molecules/panels/Drawer";
import usePage from "../../../shared/hooks/page/usePage";

const BankDetail = () => {
    const { currentComponentId, setCurrentComponentId } = usePage();

    return (
        <Drawer
            open={currentComponentId == BankDetail.id}
            onClose={() => { setCurrentComponentId(undefined); console.log('BankDetail close') }}
        >
            BankSaveForm
        </Drawer>
    );
}

BankDetail.id = 'BankDetail';
export default BankDetail;
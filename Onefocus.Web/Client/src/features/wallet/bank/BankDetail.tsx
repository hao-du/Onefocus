import Drawer from "../../../shared/components/molecules/panels/Drawer";
import usePage from "../../../shared/hooks/page/usePage";
import { BANK_COMPONENT_NAMES } from "../../constants";

const BankDetail = () => {
    const { isActiveComponent, closeComponent } = usePage();

    return (
        <Drawer
            open={isActiveComponent(BANK_COMPONENT_NAMES.BankDetail)}
            onClose={closeComponent}
        >
            BankSaveForm
        </Drawer>
    );
}
export default BankDetail;
import { useContext } from "react";
import PageContext from "./PageContext";

const usePage = () => {
    const context = useContext(PageContext);
    if (!context) {
        throw new Error('usePage must be used within the PageProvider');
    }
    return context;
};

export default usePage;
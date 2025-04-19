import {useState} from "react";

export const useWorkspace = () => {
    const [viewRightPanel, setViewRightPanel] = useState(false);

    return {viewRightPanel, setViewRightPanel}
};


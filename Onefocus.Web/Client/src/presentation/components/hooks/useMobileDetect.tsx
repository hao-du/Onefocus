import React, {createContext, useContext, useEffect, useState} from 'react';

const MobileContext = createContext<boolean>(false);

export const useMobileDetect = () => useContext(MobileContext);

export const MobileDetectProvider: React.FC<{ children: React.ReactNode }> = ({children}) => {
    const [isMobile, setIsMobile] = useState(() => window.innerWidth < 768);

    useEffect(() => {
        const handleResize = () => {
            if (window.innerWidth < 768) {
                setIsMobile(true);
            }
            else {
                setIsMobile(false);
            }
        }
        window.addEventListener('resize', handleResize);

        return () => window.removeEventListener('resize', handleResize);
    }, []);

    return (
        <MobileContext.Provider value={isMobile}>
            {children}
        </MobileContext.Provider>
    );
};
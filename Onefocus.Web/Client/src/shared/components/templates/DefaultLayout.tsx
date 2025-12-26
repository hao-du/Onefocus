import React, { useState } from 'react';

interface DefaultLayoutProps {
    header?: React.ReactNode;
    sidebar?: React.ReactNode;
    children?: React.ReactNode;
};

export default function DefaultLayout(props: DefaultLayoutProps) {
    const [sidebarOpen, setSidebarOpen] = useState(false);

    return (
        <div className="flex w-screen h-screen bg-white">
            {/* Sidebar */}
            <div
                className={`
                    fixed inset-y-0 left-0 z-1 w-3xs bg-(--ant-color-bg-layout)
                    transition-transform duration-300 ease-in-out
                    md:relative md:translate-x-0
                    ${sidebarOpen ? 'translate-x-0' : '-translate-x-full md:translate-x-0'}
                `}
            >
                {/* Close Button (Mobile/Tablet) */}
                <div className="md:hidden p-4 flex justify-end">
                    <button
                        onClick={() => setSidebarOpen(false)}
                        className="p-1 hover:bg-gray-200 rounded"
                        aria-label="Close sidebar"
                    >
                        X
                    </button>
                </div>

                {/* Sidebar Content */}
                <div className="overflow-y-auto h-full">
                    {props.sidebar}
                </div>
            </div>

            {/* Overlay (Mobile/Tablet) */}
            {sidebarOpen && (
                <div
                    className="fixed inset-0 bg-black/20 z-0 md:hidden"
                    onClick={() => setSidebarOpen(false)}
                />
            )}

            {/* Main Content */}
            <div className="flex-1 flex flex-col min-w-0">
                {/* Header */}
                <div className="flex items-center border-b border-gray-200 bg-white">
                    {/* Hamburger Button (Mobile/Tablet) */}
                    <button
                        onClick={() => setSidebarOpen(true)}
                        className="md:hidden p-4 hover:bg-gray-100"
                        aria-label="Open sidebar"
                    > X
                    </button>

                    {/* Header Content */}
                    <div className="flex-1 p-4">
                        {props.header}
                    </div>
                </div>

                {/* Workspace/Main Content */}
                <div className="flex-1 overflow-auto">
                    {props.children}
                </div>
            </div>
        </div>
    );
};
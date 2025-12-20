import { useState } from "react";
import { BaseIdentityProps } from "../../props/BaseProps";
import { TbLoader } from "react-icons/tb";


interface MainTemplateProps extends BaseIdentityProps {
};

export const MainTemplate = (props: MainTemplateProps) => {
    const [open, setOpen] = useState(false);

    return (
        <div className="flex h-dvh overflow-hidden">
            {/* Sidebar */}
            <aside
                className={`
                    fixed inset-y-0 left-0 z-50 w-64 bg-slate-900 text-white
                    transform transition-transform duration-300
                    ${open ? "translate-x-0" : "-translate-x-full"}
                    lg:static lg:translate-x-0
                `}
            >
                {/* Sidebar header */}
                <div className="flex items-center justify-between p-4 border-b border-white/10">
                    <span className="font-semibold">Sidebar</span>

                    {/* Close button → mobile only */}
                    <button
                        className="lg:hidden text-xl"
                        onClick={() => setOpen(false)}
                        aria-label="Close sidebar"
                    >
                        <TbLoader className="of-spin"/>
                    </button>
                </div>

                <nav className="p-4 space-y-2">
                    <div>Dashboard</div>
                    <div>Settings</div>
                    <div>Profile</div>
                </nav>
            </aside>

            {/* Overlay (mobile only) */}
            {open && (
                <div
                    className="fixed inset-0 z-40 bg-black/40 lg:hidden"
                    onClick={() => setOpen(false)}
                />
            )}

            {/* Right column */}
            <div className="flex flex-1 flex-col min-w-0">
                {/* Header */}
                <header className="h-14 flex items-center bg-slate-900 text-white px-4 gap-3">
                    {/* Hamburger → mobile only */}
                    <button
                        className="lg:hidden text-2xl"
                        onClick={() => setOpen(true)}
                        aria-label="Open sidebar"
                    >
                        ☰
                    </button>

                    <span className="font-semibold">Header</span>
                </header>

                {/* Workspace */}
                <main className="flex-1 overflow-auto bg-slate-100 p-4">
                    Workspace / Main Content
                </main>
            </div>
        </div>
    );
}
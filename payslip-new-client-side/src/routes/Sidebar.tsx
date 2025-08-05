import { useState } from "react";
import { Users, LogOut, Menu, UserCheck, Calculator } from "lucide-react";
import { useNavigate, useLocation } from "react-router-dom";
import { useTranslation } from "react-i18next";
import clsx from "clsx";
import LanguageSwitcher from "../components/LanguageSwitcher";

const menuItems = [
    { labelKey: "users", icon: Users, path: "/admin/users", type: 'admin' },
    { labelKey: "updatePersonalInfo", icon: UserCheck, path: "/admin/personalInfo", type: 'admin' },
    { labelKey: "payslips", icon: Calculator, path: "/admin/payslips", type: 'admin' },
    { labelKey: "updatePersonalInfo", icon: UserCheck, path: "/personalInfo", type: 'user' },
    { labelKey: "userPayslip", icon: Calculator, path: "/payslip", type: 'user' },
];

const Sidebar = ({ children }: { children: React.ReactNode }) => {
    const [isOpen, setIsOpen] = useState(true);
    const navigate = useNavigate();
    const location = useLocation();
    const { t, i18n } = useTranslation();

    const toggleSidebar = () => setIsOpen(!isOpen);

    return (
        <div className="flex min-h-screen bg-muted">
            {/* Sidebar */}
            <div
                className={clsx(
                    "fixed inset-y-0 z-40 w-64 bg-primary text-white shadow-md flex flex-col justify-between transform transition-transform duration-200 ease-in-out",
                    {
                        left: i18n.language !== "fa",
                        right: i18n.language === "fa",
                        "-translate-x-full": !isOpen && i18n.language !== "fa",
                        "translate-x-0": isOpen && i18n.language !== "fa",
                        "translate-x-full": !isOpen && i18n.language === "fa",
                    }
                )}
            >
                <div>
                    <div className="p-4 text-xl font-bold border-b border-white/20">
                        {location.pathname.includes("admin") ? t("adminPanel") : t("userPanel")}
                    </div>
                    <ul className="mt-4 space-y-1">
                        {menuItems
                            .filter(item => {
                                const isAdminRoute = location.pathname.includes("admin");
                                return isAdminRoute ? item.type === "admin" : item.type === "user";
                            })
                            .map(({ labelKey, icon: Icon, path }) => {
                                const active = location.pathname === path;
                                return (
                                    <li key={labelKey}>
                                        <button
                                            onClick={() => {
                                                navigate(path);
                                                setIsOpen(false);
                                            }}
                                            className={clsx(
                                                "flex items-center w-full px-4 py-2 text-left rounded transition-colors",
                                                active
                                                    ? "bg-secondary/80 text-white font-semibold"
                                                    : "hover:bg-secondary/20"
                                            )}
                                        >
                                            <Icon
                                                className={`w-5 h-5 ${i18n.language === "fa" ? "ml-3" : "mr-3"}`}
                                            />
                                            {t(labelKey)}
                                        </button>
                                    </li>
                                );
                            })}
                    </ul>
                </div>

                {/* Logout at bottom */}
                <div className="border-t border-white/20 p-4 flex items-center justify-between">
                    <button
                        onClick={() => {
                            localStorage.removeItem("token");
                            navigate("/admin/login");
                        }}
                        className="flex items-center px-4 py-2 text-left hover:bg-secondary/20 text-danger font-semibold rounded"
                    >
                        <LogOut className={`w-5 h-5 ${i18n.language === "fa" ? "ml-3" : "mr-3"}`} />
                        {t("logout")}
                    </button>

                    <LanguageSwitcher />
                </div>
            </div>

            {/* Content area */}
            <div className="flex-1 flex flex-col min-h-screen overflow-x-auto relative">
                {/* Top Bar */}
                <div className="flex flex-row-reverse items-center justify-between bg-primary text-white shadow px-4 py-4">
                    <button onClick={toggleSidebar}>
                        <Menu className="w-6 h-6" />
                    </button>
                    <span className="text-lg font-semibold">
                        {location.pathname.includes("admin") ? t("adminPanel") : t("userPanel")}

                    </span>
                    <div /> {/* spacer */}
                </div>

                {/* Main content */}
                <main
                    className={clsx(
                        "bg-background flex-1 p-4 mt-16 sm:mt-0 transition-all duration-300",
                        {
                            "sm:ml-64": isOpen && i18n.language !== "fa",
                            "sm:mr-64": isOpen && i18n.language === "fa",
                        }
                    )}
                >
                    {children}
                </main>
            </div>
        </div>
    );
};

export default Sidebar;

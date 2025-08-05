import { useLocation, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Users, UserCheck, Calculator } from "lucide-react";
import clsx from "clsx";

const Dashboard = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { t, i18n } = useTranslation();

  const isAdmin = location.pathname.includes("admin");

  const shortcuts = isAdmin
    ? [
        {
          labelKey: "users",
          descriptionKey: "manageUsersDesc",
          icon: Users,
          path: "/admin/users",
        },
        {
          labelKey: "updatePersonalInfo",
          descriptionKey: "adminUpdatePersonalDesc",
          icon: UserCheck,
          path: "/admin/personalInfo",
        },
        {
          labelKey: "payslips",
          descriptionKey: "viewAndEditPayslipsDesc",
          icon: Calculator,
          path: "/admin/payslips",
        },
      ]
    : [
        {
          labelKey: "updatePersonalInfo",
          descriptionKey: "userUpdatePersonalDesc",
          icon: UserCheck,
          path: "/personalInfo",
        },
        {
          labelKey: "userPayslip",
          descriptionKey: "viewPayslipsDesc",
          icon: Calculator,
          path: "/payslip",
        },
      ];

  return (
    <div className="p-6 space-y-6">
      {/* Welcome message */}
      <div className="bg-white rounded-2xl shadow px-6 py-5 border border-gray-200">
        <h1 className="text-2xl font-bold mb-1">
          {t("welcomeMessage", {
            panel: t(isAdmin ? "adminPanel" : "userPanel"),
          })}
        </h1>
        <p className="text-muted-foreground text-sm">
          {t(isAdmin ? "adminWelcomeDesc" : "userWelcomeDesc")}
        </p>
      </div>

      {/* Quick Access Cards */}
      <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
        {shortcuts.map(({ labelKey, descriptionKey, icon: Icon, path }) => (
          <button
            key={labelKey}
            onClick={() => navigate(path)}
            className="bg-white shadow-md rounded-2xl p-6 text-left transition hover:shadow-lg border border-gray-200 hover:bg-gray-50 flex flex-col items-start"
          >
            <div className="flex items-center mb-4">
              <Icon className={clsx("w-10 h-10 text-primary", i18n.language === "fa" ? "ml-4" : "mr-4")} />
              <h2 className="text-xl font-bold">{t(labelKey)}</h2>
            </div>
            <p className="text-sm text-muted-foreground">{t(descriptionKey)}</p>
          </button>
        ))}
      </div>
    </div>
  );
};

export default Dashboard;

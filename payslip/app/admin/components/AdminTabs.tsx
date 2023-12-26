"use client";
import Link from "next/link";
import { useEffect, useState } from "react";
import { usePathname } from "next/navigation";

const AdminTabs = () => {
  const [currentTab, setCurrentTab] = useState("");
  const currentPage = usePathname();

  useEffect(() => {
    if (currentPage.includes("users")) setCurrentTab("users");
    if (currentPage.includes("payslips-upload"))
      setCurrentTab("payslips-upload");
  }, []);

  return (
    <div className="p-10">
      <div className="flex flex-col sm:flex-row gap-5 w-full">
        <Link
          onClick={() => setCurrentTab("payslips-upload")}
          href="/admin/payslips-upload"
          className={`w-full sm:w-1/2 p-4 rounded text-center ease-in duration-300  shadow-md ${
            currentTab === "payslips-upload"
              ? "text-white bg-teal-700"
              : "bg-white text-teal-700 hover:bg-teal-700 hover:text-white"
          }`}
        >
          بارگذاری فیش‌های حقوقی
        </Link>
        <Link
          onClick={() => setCurrentTab("users")}
          href="/admin/users"
          className={`w-full sm:w-1/2 p-4 rounded text-center ease-in duration-300 shadow-md ${
            currentTab === "users"
              ? "text-white bg-teal-700"
              : "bg-white text-teal-700 hover:bg-teal-700 hover:text-white"
          }`}
        >
          اطلاعات کاربران
        </Link>
      </div>
    </div>
  );
};

export default AdminTabs;

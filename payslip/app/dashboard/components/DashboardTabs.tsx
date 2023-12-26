"use client";

import Link from "next/link";
import { useState } from "react";

const DashboardTabs = () => {
  const [currentTab, setCurrentTab] = useState("");
  return (
    <div className="p-10">
      <div className="flex flex-col sm:flex-row gap-5 w-full">
        <Link
          onClick={() => setCurrentTab("payslip")}
          href="/dashboard/payslip"
          className={`w-full sm:w-1/2 p-4 rounded text-center ease-in duration-300  shadow-md ${currentTab === 'payslip' ? 'text-white bg-teal-700' : 'bg-white text-teal-700 hover:bg-teal-700 hover:text-white'}`}
        >
          فیش حقوقی
        </Link>
        <Link
          onClick={() => setCurrentTab("profile")}
          href="/dashboard/profile"
          className={`w-full sm:w-1/2 p-4 rounded text-center ease-in duration-300 shadow-md ${currentTab === 'profile' ? 'text-white bg-teal-700' : 'bg-white text-teal-700 hover:bg-teal-700 hover:text-white'}`}
        >
          اطلاعات شخصی
        </Link>
      </div>
    </div>
  );
};

export default DashboardTabs;

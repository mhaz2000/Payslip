"use client";

import { Month, MonthMapper } from "@/app/Enums/Month";
import DropDown from "@/app/components/DropDown";
import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { AxiosResponse } from "axios";
import { useEffect, useState } from "react";
import UserPayslip from "./UserPayslip";

interface UserWagesModel {
  year: number;
  months: Month[];
}

const Payslip = () => {
  const axiosAuth = useAxiosAuth();

  const [userWages, setUserWages] = useState<UserWagesModel[]>([]);
  const [months, setMonths] = useState<Month[]>([]);
  const [years, setYears] = useState<number[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      const res: AxiosResponse<UserWagesModel[]> = await axiosAuth.get(
        "api/payslips/wages"
      );

      const payslipsYears = res.data.map((wage) => wage.year);
      setYears(payslipsYears);

      setUserWages(res.data);
    };

    fetchData();
  }, []);

  const [selectedMonth, setSelectedMonth] = useState<Month | null>(null);
  const [selectedYear, setSelectedYear] = useState<number | null>(null);

  const monthHandler = (value: number) => {
    setSelectedMonth(value);
  };

  const yearHandler = (value: number) => {
    setSelectedYear(value);

    setMonths(userWages.filter((wage) => wage.year == value)[0].months);
  };

  return (
    <div className="">
      <div className="flex flex-col w-full items-center">
        <h1 className="dir-right text-base lg:text-3xl md:text-xl mb-6 mt-14 mx-auto text-center">
          برای مشاهده فیش حقوقی خود سال و ماه صدور را وارد نمایید.
        </h1>
        <div className="flex flex-col md:flex-row gap-4 justify-center">
          <DropDown
            title="سال را انتخاب کنید."
            mapper={null}
            options={years as [any]}
            handler={yearHandler}
            disabled={false}
          />
          <DropDown
            title="ماه را انتخاب کنید."
            mapper={MonthMapper}
            options={months as [any]}
            handler={monthHandler}
            disabled={!selectedYear}
          />
        </div>
        <UserPayslip month={selectedMonth} year={selectedYear} />
      </div>
    </div>
  );
};

export default Payslip;

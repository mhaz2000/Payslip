"use client";

import { MonthMapper } from "@/app/Enums/Month";
import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { AxiosResponse } from "axios";
import { useEffect, useRef, useState } from "react";
import ReactToPrint from "react-to-print";

interface TableRow {
  salaryAndBenefitTitle: string;
  salaryAndBenefitAmount: string;
  salaryAndBenefitDuration: string;
  deductionTitle: string;
  deductionAmount: string;
}

const UserPayslip = ({
  month,
  year,
}: {
  month: number | null;
  year: number | null;
}) => {
  const ref = useRef<any>();

  const [payslip, setPayslip] = useState<UserPayslip | null>(null);
  const [payslipItems, setPayslipItems] = useState<TableRow[]>([]);
  const axiosAuth = useAxiosAuth();
  useEffect(() => {
    const fetchData = async () => {
      const res: AxiosResponse<UserPayslip> = await axiosAuth.get(
        `api/payslips/UserPayslip?year=${year}&month=${month}`
      );

      setPayslip(res.data);
      let maxRow = Math.max(
        Object.keys(res.data.deductions ? res.data.deductions : 0).length,
        Object.keys(res.data.salaryAndBenefits ? res.data.salaryAndBenefits : 0)
          .length
      );

      const items: TableRow[] = [];
      for (let i = 1; i <= maxRow; i++) {
        items.push({
          salaryAndBenefitTitle:
            res.data.salaryAndBenefits && res.data.salaryAndBenefits[i]
              ? res.data.salaryAndBenefits[i]
              : "",
          salaryAndBenefitAmount:
            res.data.salaryAndBenefitsAmount &&
            res.data.salaryAndBenefitsAmount[i]
              ? res.data.salaryAndBenefitsAmount[i]
              : "",
          salaryAndBenefitDuration:
            res.data.durations && res.data.durations[i]
              ? res.data.durations[i]
              : "",
          deductionTitle:
            res.data.deductions && res.data.deductions[i]
              ? res.data.deductions[i]
              : "",
          deductionAmount:
            res.data.deductionsAmount && res.data.deductionsAmount[i]
              ? res.data.deductionsAmount[i]
              : "",
        });
      }
      setPayslipItems(items);
    };

    if (year && month) fetchData();
  }, [month, year]);

  return (
    <>
      <div
        ref={ref}
        className={`text-black min-w-full p-10 mx-auto ${
          payslip ? "" : "hidden"
        }`}
      >
        <div className="bg-slate-300 rounded-lg">
          <h2 className="text-lg lg:text-xl text-center pt-5">
            شرکت پترو آرمان صنعت دانیال
          </h2>
          <h4 className="text-sm lg:text-base text-center py-3">
            صورتحساب حقوق و مزایای پرسنل ماه {MonthMapper.get(month!)} سال{" "}
            {year}
          </h4>
          <div className="text-sm lg:text-base flex flex-row justify-around my-2">
            <div className="flex flex-col">
              <div className="py-2">
                <span className="font-bold">نام: </span>
                {payslip?.firstName}
              </div>
              <div className="">
                <span className="font-bold">نوع استخدام: </span>
                {payslip?.contractType}
              </div>
            </div>
            <div className="flex flex-col">
              <div className="py-2">
                <span className="font-bold">نام خانوادگی: </span>
                {payslip?.lastName}
              </div>
              <div className="">
                <span className="font-bold">محل خدمت: </span>
                {payslip?.location}
              </div>
            </div>

            <div className="flex flex-col">
              <div className="py-2">
                <span className="font-bold">شماره کارت: </span>
                {payslip?.cardNumber}
              </div>
              <div className="">
                <span className="font-bold">سمت: </span>
                {payslip?.position}
              </div>
            </div>
          </div>
          <table className="min-w-full text-sm text-gray-200 my-3">
            <thead className="bg-teal-800 text-xs uppercase font-medium ">
              <tr>
                <th scope="col" className="px-6 py-3 text-right tracking-wider">
                  حقوق و مزایا
                </th>
                <th scope="col" className="px-6 py-3 text-right tracking-wider">
                  دقیقه:ساعت:روز
                </th>
                <th scope="col" className="px-6 py-3 text-right tracking-wider">
                  مبلغ
                </th>
                <th scope="col" className="px-6 py-3 text-right tracking-wider">
                  کسورات
                </th>
                <th scope="col" className="px-6 py-3 text-right tracking-wider">
                  مبلغ
                </th>
                <th
                  colSpan={2}
                  scope="col"
                  className="px-6 py-3 text-right tracking-wider"
                >
                  توضیحات
                </th>
              </tr>
            </thead>
            <tbody className="bg-transparent text-black">
              {payslipItems.map((item, index) => (
                <tr
                  key={index}
                  className={`bg-slate-200 ${
                    index % 2 === 1 ? "bg-teal-50" : ""
                  }`}
                >
                  <td className="pr-7 my-2">{item.salaryAndBenefitTitle}</td>
                  <td className="pr-7 my-2 dir-left flex justify-end">
                    {item.salaryAndBenefitDuration}
                  </td>
                  <td className="pr-7 my-2">{item.salaryAndBenefitAmount}</td>
                  <td className="pr-7 my-2">{item.deductionTitle}</td>
                  <td className="pr-7 my-2">{item.deductionAmount}</td>
                  <td className="pr-7 my-2"></td>
                  <td className="pr-7 my-2"></td>
                </tr>
              ))}
            </tbody>
            <tfoot className="bg-transparent text-black border-t border-black font-bold">
              <tr
                className={`bg-slate-200 h-8 ${
                  payslipItems.length % 2 === 0
                } ? "bg-teal-50" : ""`}
              >
                <td colSpan={2} className="pr-7 my-2">
                  جمع کل حقوق و مزایا
                </td>
                <td className="pr-7 my-2">{payslip?.totalSalaryAndBenefits}</td>
                <td className="pr-7 my-2">جمع کل کسور</td>
                <td className="pr-7 my-2">{payslip?.totalDeductions}</td>
                <td className="pr-7 my-2">خالص پرداختی</td>
                <td className="pr-7 my-2">{payslip?.netPayable}</td>
              </tr>
            </tfoot>
          </table>
        </div>
      </div>
      {
        payslip && 
        <ReactToPrint
          bodyClass="print-payslip"
          content={() => ref.current}
          trigger={() => <button className="btn-style mb-5">چاپ فیش حقوقی</button>}
        />
      }
    </>
  );
};

export default UserPayslip;

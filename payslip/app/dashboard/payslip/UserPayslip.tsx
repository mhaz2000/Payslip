"use client";

import { MonthMapper } from "@/app/Enums/Month";
import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { AxiosResponse } from "axios";
import Image from "next/image";
import { useEffect, useRef, useState } from "react";
import ReactToPrint from "react-to-print";

interface TableRow {
  salaryAndBenefitTitle: string;
  salaryAndBenefitAmount: string;
  salaryAndBenefitDuration: string;
  deductionTitle: string;
  deductionAmount: string;
  descriptionTitle: string;
  descriptionAmount: string;
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
          descriptionTitle:
            res.data.descriptions && res.data.descriptions[i]
              ? res.data.descriptions[i]
              : "",
          descriptionAmount:
            res.data.descriptionsAmount && res.data.descriptionsAmount[i]
              ? res.data.descriptionsAmount[i]
              : "",
        });
      }
      setPayslipItems(items);
    };

    if (year && month) fetchData();
  }, [month, year]);

  return (
    <>
      <div className="hidden">
        <div ref={ref} className={`text-black min-w-full px-5 mx-auto`}>
          <div className="pt-2">
            <div className="flex flex-row justify-around items-center">
              <div className="w-1/3 flex justify-start">
                <Image src="/logo.png" alt="Logo" width={100} height={50} />
              </div>
              <div className="w-1/3">
                <h2 className="lg:text-base text-sm text-center pt-5 font-IranSansBold">
                  شرکت پترو آرمان صنعت دانیال
                </h2>
                <h4 className="text-2xs lg:text-xs text-center py-3">
                  فیش حقوق و مزایای پرسنل ماه {MonthMapper.get(month!)} سال{" "}
                  {year}
                </h4>
              </div>
              <div className="w-1/3"></div>
            </div>
            <div className="text-2xs lg:text-sm flex flex-row justify-between my-2">
              <div className="flex flex-col w-1/3">
                <div className="py-2">
                  <span className="font-bold">نام: </span>
                  {payslip?.firstName}
                </div>
                <div className="">
                  <span className="font-bold">نوع استخدام: </span>
                  {payslip?.contractType}
                </div>
              </div>
              <div className="flex flex-col w-1/3">
                <div className="py-2 mx-auto">
                  <span className="font-bold">نام خانوادگی: </span>
                  {payslip?.lastName}
                </div>
                <div className="mx-auto">
                  <span className="font-bold">محل خدمت: </span>
                  {payslip?.location}
                </div>
              </div>
              <div className="flex flex-col w-1/3 text-left items-end">
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
            <table className="border-collapse border border-black min-w-full text-xs lg:text-sm text-gray-200 my-3">
              <thead className="border-collapse border border-black text-black text-2xs uppercase font-medium ">
                <tr>
                  <th
                    scope="col"
                    className="border-collapse border-l border-black px-2 py-1 text-right tracking-wider"
                  >
                    حقوق و مزایا
                  </th>
                  <th
                    scope="col"
                    className="border-collapse border-x border-black px-2 py-1 text-right tracking-wider"
                  >
                    دقیقه:ساعت:روز
                  </th>
                  <th
                    scope="col"
                    className="border-collapse border-x border-black px-2 py-1 text-right tracking-wider"
                  >
                    مبلغ
                  </th>
                  <th
                    scope="col"
                    className="border-collapse border-x border-black px-3 py-3 text-right tracking-wider"
                  >
                    کسورات
                  </th>
                  <th
                    scope="col"
                    className="border-collapse border-x border-black px-2 py-1 text-right tracking-wider"
                  >
                    مبلغ
                  </th>
                  <th
                    colSpan={2}
                    scope="col"
                    className="border-collapse border-x border-black px-2 py-1 text-right tracking-wider"
                  >
                    توضیحات
                  </th>
                </tr>
              </thead>
              <tbody className="bg-transparent text-black">
                {payslipItems.map((item, index) => (
                  <tr className="h-7" key={index}>
                    <td className="border-collapse border-x border-black px-3 my-1 text-2xs lg:text-xs">
                      {item.salaryAndBenefitTitle}
                    </td>
                    <td className="px-3 my-1 dir-left flex justify-end text-2xs lg:text-xs">
                      {item.salaryAndBenefitDuration}
                    </td>
                    <td className="border-collapse border-x border-black px-3 my-1 text-2xs lg:text-xs">
                      {item.salaryAndBenefitAmount}
                    </td>
                    <td className="border-collapse border-x border-black px-3 my-1 text-2xs lg:text-xs">
                      {item.deductionTitle}
                    </td>
                    <td className="border-collapse border-x border-black px-3 my-1 text-2xs lg:text-xs">
                      {item.deductionAmount}
                    </td>
                    <td className="border-collapse border-x border-black px-3 my-1 text-2xs lg:text-xs">
                      {item.descriptionTitle}
                    </td>
                    <td className="border-collapse border-x border-black px-3 my-1 text-2xs lg:text-xs">
                      {item.descriptionAmount}
                    </td>
                  </tr>
                ))}
              </tbody>
              <tfoot className="bg-transparent text-black border-t border-black">
                <tr className={`h-8`} style={{ fontSize: "10px" }}>
                  <td colSpan={2} className="px-3 my-2 text-xss">
                    جمع کل حقوق و مزایا
                  </td>
                  <td className="border-collapse border-x border-black px-3 my-2 text-xss">
                    {payslip?.totalSalaryAndBenefits}
                  </td>
                  <td className="px-3 my-2 text-xss">جمع کل کسور</td>
                  <td className="border-collapse border-x border-black px-3 my-2 text-xss">
                    {payslip?.totalDeductions}
                  </td>
                  <td className="px-3 my-2 text-xss">خالص پرداختی</td>
                  <td className="border-collapse border-x border-black px-3 my-2 text-xss">
                    {payslip?.netPayable}
                  </td>
                </tr>
              </tfoot>
            </table>
          </div>
        </div>
      </div>

      <div
        className={`text-black min-w-full p-10 mx-auto ${
          payslip ? "" : "hidden"
        }`}
      >
        <div className="bg-slate-300 rounded-lg">
          <div className="flex flex-row justify-around items-center">
            <div className="w-1/3 flex justify-center">
              <Image src="/logo.png" alt="Logo" width={100} height={50} />
            </div>
            <div className="w-1/3">
              <h2 className="text-lg lg:text-xl text-center pt-5">
                شرکت پترو آرمان صنعت دانیال
              </h2>
              <h4 className="text-sm lg:text-base text-center py-3">
                فیش حقوق و مزایای پرسنل ماه {MonthMapper.get(month!)} سال {year}
              </h4>
            </div>
            <div className="w-1/3"></div>
          </div>
          <div className="text-sm lg:text-base flex flex-row justify-around my-2">
            <div className="flex flex-col w-1/3">
              <div className="py-2 mx-auto">
                <span className="font-bold">نام: </span>
                {payslip?.firstName}
              </div>
              <div className="mx-auto">
                <span className="font-bold">نوع استخدام: </span>
                {payslip?.contractType}
              </div>
            </div>
            <div className="flex flex-col w-1/3">
              <div className="py-2 mx-auto">
                <span className="font-bold">نام خانوادگی: </span>
                {payslip?.lastName}
              </div>
              <div className="mx-auto">
                <span className="font-bold">محل خدمت: </span>
                {payslip?.location}
              </div>
            </div>
            <div className="flex flex-col w-1/3">
              <div className="py-2 mx-auto">
                <span className="font-bold">شماره کارت: </span>
                {payslip?.cardNumber}
              </div>
              <div className="mx-auto">
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
                  className={`bg-slate-200 h-8 ${
                    index % 2 === 1 ? "bg-teal-50" : ""
                  }`}
                >
                  <td className="pr-7 my-1">{item.salaryAndBenefitTitle}</td>
                  <td className="pr-7 my-1 dir-left flex justify-end">
                    {item.salaryAndBenefitDuration}
                  </td>
                  <td className="pr-7 my-1">{item.salaryAndBenefitAmount}</td>
                  <td className="pr-7 my-1">{item.deductionTitle}</td>
                  <td className="pr-7 my-1">{item.deductionAmount}</td>
                  <td className="pr-7 my-1">{item.descriptionTitle}</td>
                  <td className="pr-7 my-1">{item.descriptionAmount}</td>
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
      {payslip && (
        <ReactToPrint
          bodyClass="print-payslip"
          content={() => ref.current}
          trigger={() => (
            <button className="btn-style mb-5 text-gray-300">
              چاپ فیش حقوقی
            </button>
          )}
        />
      )}
    </>
  );
};

export default UserPayslip;

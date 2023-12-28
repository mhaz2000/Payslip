"use client";
import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { displayError } from "@/lib/toastDisplay";
import { AxiosResponse } from "axios";
import { useEffect, useState } from "react";

interface Response {
  total: number;
  data: PayslipModel[];
}

const PayslipsList = () => {
  const axiosAuth = useAxiosAuth();
  const [refresh, setRefresh] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [payslips, setPayslips] = useState<PayslipModel[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response: AxiosResponse<Response> = await axiosAuth.get(
          "payslips"
        );

        setPayslips(response.data.data);
      } catch (error: any) {
        displayError(error.data.message);
      }
    };

    fetchData();
  }, []);

  const removePayslip = (payslipId: string) => {};

  const downloadPayslipFile = (payslipId: string) => {};

  return (
    <>
      {isLoading ? (
        <p className="my-5 text-slate-400 mr-2">در حال دریافت اطلاعات</p>
      ) : payslips.length > 1 ? (
        <tbody className="bg-table">
          {payslips.map((payslip, index) => (
            <tr
              className={`bg-black ${index % 2 === 0 ? "bg-opacity-20" : ""}`}
              key={payslip.id}
            >
              <td className="pr-7">{index + 1}</td>
              <td className="flex px-6 py-4 whitespace-nowrap">
                <span className="ml-2 font-medium">{payslip.uploadedDate}</span>
              </td>
              <td className="px-6 py-4 whitespace-nowrap">{payslip.year}</td>
              <td className="px-6 py-4 whitespace-nowrap">{payslip.month}</td>
              <td className="px-6 py-4 whitespace-nowrap">
                <div className="flex flex-row gap-2">
                  <button
                    className="btn-style w-24 bg-black border-red-500 text-red-500 hover:bg-red-500"
                    onClick={() => removePayslip(payslip.id)}
                  >
                    حذف
                  </button>
                  <button
                    onClick={() => downloadPayslipFile(payslip.id)}
                    className={`btn-style w-24 bg-black border-blue-500 text-blue-500 hover:bg-blue-500`}
                  >
                    دانلود
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      ) : (
        <p className="my-5 text-slate-400 mr-2">اطلاعاتی یافت نشد.</p>
      )}
    </>
  );
};

export default PayslipsList;

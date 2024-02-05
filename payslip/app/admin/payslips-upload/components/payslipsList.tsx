"use client";
import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { displayError, displaySuccess } from "@/lib/toastDisplay";
import axios, { AxiosResponse } from "axios";
import { useEffect, useState } from "react";

interface Response {
  total: number;
  data: PayslipModel[];
}

interface PayslipsListComponentProps {
  handleCount: (count: number) => void;
  current: number;
  gridRefresh: boolean;
}

const PayslipsList = ({
  handleCount,
  current,
  gridRefresh,
}: PayslipsListComponentProps) => {
  const axiosAuth = useAxiosAuth();
  const [refresh, setRefresh] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [payslips, setPayslips] = useState<PayslipModel[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response: AxiosResponse<Response> = await axiosAuth.get(
          `/api/payslips?skip=${(current - 1) * 10}`
        );

        setPayslips(response.data.data);
        handleCount(response.data.total);
      } catch (error: any) {
        displayError(error.data.message);
      }
      setIsLoading(false);
    };
    setIsLoading(true);

    setTimeout(() => {
      fetchData();
    }, 200);
  }, [current, refresh, gridRefresh]);

  const removePayslip = async (payslipId: string) => {
    try {
      await axiosAuth.delete(`api/payslips/${payslipId}`);
      setRefresh((prev) => (prev = !prev));
    } catch (error: any) {
      displayError(error.data.message);
    }

    displaySuccess("عملیات با موفقیت انجام شد.");
  };

  const downloadPayslipFile = async (
    payslipId: string,
    month: number,
    year: string
  ) => {
    let res = await axiosAuth.get(`api/files/${payslipId}`, {
      responseType: "arraybuffer",
      headers: {
        "Content-Disposition": `attachment; filename=file.xlsx`,
        "Content-Type": "application/octet-stream",
      },
    });

    const blob = new Blob([res.data], { type: "application/octet-stream" });

    const url = URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = `${month}-${year}.xlsx`;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
  };

  return (
    <>
      {isLoading ? (
        <tbody className="block m-5">
          <tr className="my-5 text-slate-400 mr-2">
            <td>در حال دریافت اطلاعات</td>
          </tr>
        </tbody>
      ) : payslips.length > 0 ? (
        <tbody className="bg-table">
          {payslips.map((payslip, index) => (
            <tr
              className={`bg-black ${index % 2 === 0 ? "bg-opacity-20" : ""}`}
              key={payslip.fileId}
            >
              <td className="pr-7">{index + 1}</td>
              <td className="flex px-6 py-4 whitespace-nowrap">
                <span className="ml-2 font-medium">{payslip.uploadedDate}</span>
              </td>
              <td className="px-6 py-4 whitespace-nowrap">{payslip.year}</td>
              <td className="px-6 py-4 whitespace-nowrap">{payslip.month}</td>
              <td className="px-6 py-4 whitespace-nowrap">
                <span className="flex flex-row gap-2">
                  <button
                    className="btn-style w-24 bg-black border-red-500 !text-red-500 hover:bg-red-500 hover:!text-black"
                    onClick={() => removePayslip(payslip.fileId)}
                  >
                    حذف
                  </button>
                  <button
                    onClick={() =>
                      downloadPayslipFile(
                        payslip.fileId,
                        payslip.year,
                        payslip.month
                      )
                    }
                    className={`btn-style w-24 bg-black border-blue-500 !text-blue-500 hover:bg-blue-500 hover:!text-black`}
                  >
                    دانلود
                  </button>
                </span>
              </td>
            </tr>
          ))}
        </tbody>
      ) : (
        <tbody className="block m-5">
          <tr className="my-5 text-slate-400 mr-2">
            <td>اطلاعاتی یافت نشد.</td>
          </tr>
        </tbody>
      )}
    </>
  );
};

export default PayslipsList;

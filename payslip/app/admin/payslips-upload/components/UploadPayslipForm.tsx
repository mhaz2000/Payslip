"use client";

import { useState } from "react";
import DropDown from "../../../components/DropDown";
import { Month, MonthMapper } from "@/app/Enums/Month";
import Uploader from "../../../components/Uploader";
import { displayError, displaySuccess } from "@/lib/toastDisplay";
import useAxiosAuth from "@/lib/hooks/useAxiosAuth";

interface UploadPayslipFormProps {
  closeModal: () => void;
  handleAction: () => void;
}

const UploadPayslipsForm: React.FC<UploadPayslipFormProps> = ({
  closeModal,
  handleAction,
}) => {
  let years = [];
  let months = [
    Month.Farvardin,
    Month.Ordibehesht,
    Month.Khordad,
    Month.Tir,
    Month.Mordad,
    Month.Shahrivar,
    Month.Mehr,
    Month.Aban,
    Month.Azar,
    Month.Dey,
    Month.Esfand,
  ];

  const axiosAuth = useAxiosAuth(true);

  const [file, setFile] = useState<Blob | null>(null);
  const [month, setMonth] = useState<Month | null>(null);
  const [year, setYear] = useState<number | null>(null);

  let today = new Date().toLocaleDateString("fa-IR-u-nu-latn");
  for (let i: number = 1400; i <= parseInt(today.split("/")[0]); i = i + 1)
    years[i - 1400] = i;

  const fileHandler = (file: any) => {
    setFile(file);
  };

  const monthHandler = (month: Month) => {
    setMonth(month);
  };

  const yearHandler = (year: number) => {
    setYear(year);
  };

  const handleSubmit = async () => {
    if (!month) displayError("لطفا ماه را وارد کنید.");
    else if (!year) displayError("لطفا سال را وارد کنید.");
    else if (!file) displayError("لطفا فایل را بارگذاری نمایید.");

    var bodyFormData = new FormData();
    bodyFormData.append("file", file as Blob);
    try {
      const res = await axiosAuth.post(
        `/api/payslips?year=${year}&month=${month}`,
        bodyFormData
      );

      displaySuccess("فایل با موفقیت بارگذاری شد.");
      closeModal();
      handleAction();
    } catch (error: any) {

      displayError(error.data.message);
    }
  };

  return (
    <div className="relative p-6 flex-auto">
      <div className="flex flex-col gap-4 mx-auto text-sm px-5 relative justify-center">
        <Uploader fileHandler={fileHandler} />
        <div className="flex flex-col sm:flex-row justify-center mx-auto">
          <DropDown
            title="سال مالی"
            options={years as [any]}
            mapper={null}
            handler={yearHandler}
            disabled={false}
          />
          <DropDown
            title="ماه"
            options={months as [any]}
            mapper={MonthMapper}
            handler={monthHandler}
            disabled={false}
          />
        </div>
        <div className="flex items-center justify-end py-6 px-2 border-t border-solid border-blueGray-200 rounded-b gap-4">
          <div className="mx-auto flex flex-row gap-4">
            <button
              className="btn-style bg-transparent border-red-500 !text-red-500 hover:bg-red-500 w-24 hover:!text-black"
              type="submit"
              onClick={() => closeModal()}
            >
              بستن
            </button>
            <button
              className="btn-style bg-transparent border-green-500 !text-green-500 hover:bg-green-500 w-24 hover:!text-black"
              onClick={handleSubmit}
            >
              ثبت
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UploadPayslipsForm;
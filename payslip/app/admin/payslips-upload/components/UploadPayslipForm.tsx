"use client";

import { FormEvent } from "react";
import DropDown from "./DropDown";
import { Month, MonthMapper } from "@/app/Enums/Month";
import Uploader from "./Uploader";

interface UploadPayslipFormProps {
  closeModal: () => void;
  handleCreatePayslip: () => void;
}

const UploadPayslipsForm: React.FC<UploadPayslipFormProps> = ({
  closeModal,
  handleCreatePayslip,
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

  let today = new Date().toLocaleDateString("fa-IR-u-nu-latn");
  for (let i: number = 1400; i <= parseInt(today.split("/")[0]); i = i + 1)
    years[i - 1400] = i;

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    debugger;
    e.preventDefault();

    const formData = new FormData(e.currentTarget);
  };

  return (
    <div className="relative p-6 flex-auto">
      <form
        className="flex flex-col gap-4 mx-auto text-sm px-5 relative justify-center"
        onSubmit={handleSubmit}
      >
        <Uploader />
        <div className="flex flex-col sm:flex-row justify-center mx-auto">
          <DropDown title="سال مالی" options={years as [any]} mapper={null} />
          <DropDown
            title="ماه"
            options={months as [any]}
            mapper={MonthMapper}
          />
        </div>
      </form>
    </div>
  );
};

export default UploadPayslipsForm;

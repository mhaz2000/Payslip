"use client";

import TablePagination from "@/app/components/TablePagination";
import UploadedPayslips from "./components/uploadedPayslips";
import { useState } from "react";
import UploadNewPayslip from "./components/uploadNewPayslip";

const PayslipsUpload = () => {
  const [current, setCurrent] = useState(1);
  const [count, setCount] = useState(0);
  const [gridRefresh, setGridRefresh] = useState(true);

  const handleCount = (count: number) => {
    setCount(Math.ceil(count / 10));
  };

  const handlePageChange = (current: number) => {
    setCurrent(current);
  };

  const handleCreatePayslip = () => {
    setGridRefresh((prev) => !prev);
  };

  return (
    <div className="">
      <div className="flex flex-row w-full px-10">
        <UploadNewPayslip handleCreatePayslip={handleCreatePayslip} />
      </div>
      <UploadedPayslips
        handleCount={handleCount}
        current={current}
        gridRefresh={gridRefresh}
      />
      <TablePagination
        pageChangeHandler={handlePageChange}
        count={count}
        current={current}
      />
    </div>
  );
};

export default PayslipsUpload;

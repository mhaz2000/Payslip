import PayslipsList from "./payslipsList";

const UploadedPayslips = () => {
  return (
    <div className="flex flex-col justify-center mx-10  py-5">
      <div className="flex flex-col mt-6">
        <div className="-my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
          <div className="py-2 align-middle inline-block min-w-full sm:px-6 lg:px-8">
            <div className="shadow overflow-hidden sm:rounded-lg">
              <table className="min-w-full text-sm text-gray-300">
                <thead className="bg-table text-xs uppercase font-medium ">
                  <tr>
                    <th
                      scope="col"
                      className="px-6 py-3 text-right tracking-wider"
                    >
                      ردیف
                    </th>
                    <th
                      scope="col"
                      className="px-6 py-3 text-right tracking-wider"
                    >
                      تاریخ بارگذاری
                    </th>
                    <th
                      scope="col"
                      className="px-6 py-3 text-right tracking-wider"
                    >
                      سال مالی
                    </th>
                    <th
                      scope="col"
                      className="px-6 py-3 text-right tracking-wider"
                    >
                      ماه
                    </th>
                    <th
                      scope="col"
                      className="px-6 py-3 text-right tracking-wider"
                    >
                      عملیات
                    </th>
                  </tr>
                </thead>
                <PayslipsList />
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UploadedPayslips;

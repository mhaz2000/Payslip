"use client";

import { useRef, useState } from "react";
import useOnClickOutside from "use-onclickoutside";
import UploadPayslipsForm from "../../payslips-upload/components/UploadPayslipForm";
import Uploader from "@/app/components/Uploader";
import { displayError, displaySuccess } from "@/lib/toastDisplay";
import useAxiosAuth from "@/lib/hooks/useAxiosAuth";

interface UploadUserExcelProps {
  handleCreateUser: () => void;
}

const UploadUserExcel: React.FC<UploadUserExcelProps> = ({
  handleCreateUser,
}) => {
  const axiosAuth = useAxiosAuth(true);

  const [showModal, setShowModal] = useState(false);
  const modalRef = useRef<HTMLDivElement>(null);

  useOnClickOutside(modalRef, () => setShowModal(false));

  const closeModal = () => {
    setShowModal(false);
  };

  const [file, setFile] = useState<Blob | null>(null);
  const fileHandler = (file: any) => {
    setFile(file);
  };

  const sendRequest = async () => {
    if (!file) displayError("لطفا فایل را بارگذاری نمایید.");

    var bodyFormData = new FormData();
    bodyFormData.append("file", file as Blob);
    try {
      const res = await axiosAuth.post(
        `/api/users/UploadUsersFile`,
        bodyFormData
      );

      displaySuccess("فایل با موفقیت بارگذاری شد.");
      closeModal();
      handleCreateUser();
    } catch (error: any) {
      displayError(error.data.message || error.data );
    }
  };

  return (
    <>
      <button className="btn-style w-64" onClick={() => setShowModal(true)}>
        بارگذاری اشخاص
      </button>
      {showModal ? (
        <>
          <div className="justify-center items-center flex overflow-x-hidden overflow-y-auto fixed inset-0 z-50 outline-none focus:outline-none mx-10">
            <div
              ref={modalRef}
              className="relative my-6 mx-auto md:w-1/2 xl:w-1/3 max-w-3xl w-full"
            >
              <div
                className="border-gray-600 border-2 rounded-lg shadow-lg relative flex flex-col w-full bg-white outline-none focus:outline-none"
                style={{ backgroundColor: "#092635" }}
              >
                <div className="flex items-start justify-between p-5 border-b border-solid border-blueGray-200 rounded-t">
                  <h3 className="text-3xl font-semibold">
                    بارگذاری فایل اکسل اشخاص
                  </h3>
                  <button
                    className="p-1 ml-auto bg-transparent border-0 text-black opacity-5 float-right text-3xl leading-none font-semibold outline-none focus:outline-none"
                    onClick={() => setShowModal(false)}
                  >
                    <span className="bg-transparent text-black opacity-5 h-6 w-6 text-2xl block outline-none focus:outline-none">
                      ×
                    </span>
                  </button>
                </div>
                {/*body*/}
                <div className="m-5">
                  <Uploader fileHandler={fileHandler} />
                </div>
                {/*footer*/}
                <div className="flex items-center justify-end p-6 border-t border-solid border-blueGray-200 rounded-b gap-4">
                  <div className="mx-auto flex flex-row gap-4">
                    <button
                      className="btn-style bg-transparent border-red-500 text-red-500 hover:bg-red-500 w-24"
                      type="submit"
                      onClick={() => setShowModal(false)}
                    >
                      بستن
                    </button>
                    <button
                      className="btn-style bg-transparent border-green-500 text-green-500 hover:bg-green-500 w-24"
                      onClick={sendRequest}
                    >
                      ثبت
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </>
      ) : null}
    </>
  );
};

export default UploadUserExcel;

"use client";
import { useRef, useState } from "react";
import useOnClickOutside from "use-onclickoutside";
import UploadPayslipsForm from "./UploadPayslipForm";

interface UploadNewPayslipProps {
  handleCreatePayslip: () => void;
}
const UploadNewPayslip: React.FC<UploadNewPayslipProps> = ({
  handleCreatePayslip,
}) => {
  const [showModal, setShowModal] = useState(false);
  const modalRef = useRef<HTMLDivElement>(null);

  useOnClickOutside(modalRef, () => setShowModal(false));

  const closeModal = () => {
    setShowModal(false);
  };
  return (
    <>
      <button
        className="btn-style w-52 ml-2 text-gray-300"
        onClick={() => setShowModal(true)}
      >
        بارگذاری فیش حقوقی
      </button>
      {showModal ? (
        <>
          <div className="justify-center items-center flex overflow-x-hidden overflow-y-auto fixed inset-0 z-50 outline-none focus:outline-none mx-10">
            <div
              ref={modalRef}
              className="relative w-auto my-6 mx-auto lg:max-w-4xl max-w-3xl"
            >
              <div
                className="border-gray-600 border-2 rounded-lg shadow-lg relative flex flex-col w-full bg-white outline-none focus:outline-none"
                style={{ backgroundColor: "#092635" }}
              >
                <div className="flex items-start justify-between p-5 border-b border-solid border-blueGray-200 rounded-t">
                  <h3 className="text-3xl font-semibold text-gray-300">
                    بارگذاری فیش حقوقی
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
                <UploadPayslipsForm
                  closeModal={closeModal}
                  handleAction={handleCreatePayslip}
                />
                {/*footer*/}
              </div>
            </div>
          </div>
          <div className="opacity-25 fixed inset-0 z-40 bg-black"></div>
        </>
      ) : null}
    </>
  );
};

export default UploadNewPayslip;

"use client";

import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { displayError, displaySuccess } from "@/lib/toastDisplay";
import { FormEvent, useRef, useState } from "react";
import useOnClickOutside from "use-onclickoutside";

const ChangeUserPassword = ({ userId }: { userId: string }) => {
  const axiosAuth = useAxiosAuth();

  const [passwordEmpty, setPasswordEmpty] = useState(false);
  const [passwordRepEmpty, setPasswordRepEmpty] = useState(false);

  const [showModal, setShowModal] = useState(false);
  const modalRef = useRef<HTMLDivElement>(null);

  useOnClickOutside(modalRef, () => setShowModal(false));

  const changePasswordRequest = async (userId: string, newPassword: string) => {
    try {
      const res = await axiosAuth.put(
        `/api/Authentication/ChangePasswordByAdmin`,
        {
          userId: userId,
          newPassword: newPassword,
        }
      );

      displaySuccess("رمز عبور با موفقیت تغییر یافت.");
      setShowModal(false);
    } catch (error: any) {
      displayError(error.data.message);
    }
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const formData = new FormData(e.currentTarget);

    const newPassword = formData.get("newPassword");
    const newPasswordRep = formData.get("newPasswordRep");

    if (
      newPassword &&
      newPasswordRep &&
      (newPassword.valueOf() as string).length &&
      (newPasswordRep.valueOf() as string).length
    ) {
      if (newPassword !== newPasswordRep)
        displayError("رمز عبور و تکرار آن یکسان نیست.");
      else {
        await changePasswordRequest(userId, newPassword.valueOf() as string);
      }
    } else {
      if (!newPassword || (newPassword.valueOf() as string).trim().length === 0)
        setPasswordEmpty(true);

      if (
        !newPasswordRep ||
        (newPasswordRep.valueOf() as string).trim().length === 0
      )
        setPasswordRepEmpty(true);
    }
  };

  return (
    <>
      <button
        className="btn-style w-36 border-blue-500 text-blue-500 hover:bg-blue-500"
        onClick={() => setShowModal(true)}
      >
        تغییر رمز عبور
      </button>
      {showModal ? (
        <>
          <div className="justify-center items-center flex overflow-x-hidden overflow-y-auto fixed inset-0 z-50 outline-none focus:outline-none mx-10">
            <div
              ref={modalRef}
              className="relative my-6 mx-auto md:w-1/2 lg:w-1/3 xl:w-1/4 max-w-3xl w-full"
            >
              <div
                className="border-gray-600 border-2 rounded-lg shadow-lg relative flex flex-col w-full bg-white outline-none focus:outline-none"
                style={{ backgroundColor: "#092635" }}
              >
                <div className="flex items-start justify-between p-5 border-b border-solid border-blueGray-200 rounded-t">
                  <h3 className="text-3xl font-semibold">تغییر رمز عبور</h3>
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
                <form
                  className="flex flex-col gap-10 text-sm relative m-5"
                  onSubmit={handleSubmit}
                >
                  <div className="flex flex-col w-full gap-2">
                    <label className="text-right">رمز عبور</label>
                    <input
                      name="newPassword"
                      type="password"
                      onChange={(e) => {
                        if (e.target.value.length > 0) setPasswordEmpty(false);
                      }}
                      className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto ${
                        passwordEmpty ? "bg-red-400" : "bg-slate-200"
                      }`}
                    />
                  </div>
                  <div className="flex flex-col w-full gap-2">
                    <label className="text-right">تکرار رمز عبور</label>
                    <input
                      name="newPasswordRep"
                      type="password"
                      onChange={(e) => {
                        if (e.target.value.length > 0)
                          setPasswordRepEmpty(false);
                      }}
                      className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto ${
                        passwordRepEmpty ? "bg-red-400" : "bg-slate-200"
                      }`}
                    />
                  </div>

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
                        type="submit"
                      >
                        ثبت
                      </button>
                    </div>
                  </div>
                </form>

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

export default ChangeUserPassword;

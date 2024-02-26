"use client";

import "react-toastify/ReactToastify.css";

import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { FormEvent, useState } from "react";
import { displayError, displaySuccess } from "@/lib/toastDisplay";
import { useRouter } from "next/navigation";
import { signOut } from "next-auth/react";

interface ProfileProps {
  shouldSignOut: boolean;
}

const Profile = ({ shouldSignOut }: ProfileProps) => {
  const axiosAuth = useAxiosAuth();

  const [prevPassEmpty, setPrevPassEmpty] = useState(false);
  const [newPassEmpty, setNewPassEmpty] = useState(false);
  const [newPassRepEmpty, setNewPassRepEmpty] = useState(false);
  const router = useRouter();

  const changePasswordRequest = async (
    oldPassword: string,
    newPassword: string
  ) => {
    try {
      const res = await axiosAuth.put(`/api/Authentication/ChangePassword`, {
        oldPassword: oldPassword,
        newPassword: newPassword,
      });

      displaySuccess("رمز عبور با موفقیت تغییر یافت.");

      if (!shouldSignOut) router.push("/dashboard/payslip");
      else signOut();
    } catch (error: any) {
      displayError(error.data.message);
    }
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const formData = new FormData(e.currentTarget);
    const oldPassword = formData.get("oldPassword");
    const newPassword = formData.get("newPassword");
    const newPasswordRep = formData.get("newPasswordRep");

    if (
      oldPassword &&
      newPassword &&
      newPasswordRep &&
      (oldPassword.valueOf() as string).length &&
      (newPassword.valueOf() as string).length &&
      (newPasswordRep.valueOf() as string).length
    ) {
      if (newPassword !== newPasswordRep)
        displayError("رمز عبور و تکرار آن یکسان نیست.");
      else {
        await changePasswordRequest(
          oldPassword.valueOf() as string,
          newPassword.valueOf() as string
        );
      }
    } else {
      if (!oldPassword || (oldPassword.valueOf() as string).trim().length === 0)
        setPrevPassEmpty(true);

      if (!newPassword || (newPassword.valueOf() as string).trim().length === 0)
        setNewPassEmpty(true);

      if (
        !newPasswordRep ||
        (newPasswordRep.valueOf() as string).trim().length === 0
      )
        setNewPassRepEmpty(true);
    }
  };
  return (
    <div className="bg-cyan-950 lg:w-1/3 md:w-1/2 mx-auto w-5/6 rounded-xl shadow-2xl shadow-black">
      <form
        className="flex flex-col gap-2 mx-auto mt-10 max-w-md text-sm px-5 pt-5 pb-12 relative text-gray-300"
        onSubmit={handleSubmit}
      >
        <label className="mt-10 text-right">رمز عبور قبلی</label>
        <input
          name="oldPassword"
          type="password"
          onChange={(e) => {
            if (e.target.value.length > 0) setPrevPassEmpty(false);
          }}
          className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto
          ${prevPassEmpty ? "bg-red-400" : "bg-slate-200"}`}
        />
        <label className="text-right mt-3">رمز عبور جدید</label>
        <input
          name="newPassword"
          type="password"
          onChange={(e) => {
            if (e.target.value.length > 0) setNewPassEmpty(false);
          }}
          className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto
          ${newPassEmpty ? "bg-red-400" : "bg-slate-200"}`}
        />
        <label className="text-right mt-3">تکرار عبور جدید</label>
        <input
          name="newPasswordRep"
          type="password"
          onChange={(e) => {
            if (e.target.value.length > 0) setNewPassRepEmpty(false);
          }}
          className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto
          ${newPassRepEmpty ? "bg-red-400" : "bg-slate-200"}`}
        />
        <button
          type="submit"
          className="border rounded-lg h-9 w-44 mx-auto my-5 hover:bg-gray-300 hover:border-slate-900 hover:text-slate-900 ease-in duration-300"
        >
          ارسال
        </button>
      </form>
    </div>
  );
};

export default Profile;

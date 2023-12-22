"use client";

import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { FormEvent, useState } from "react";

const Profile = () => {
  const axiosAuth = useAxiosAuth();

  const [prevPassEmpty, setPrevPassEmpty] = useState(false);
  const [newPassEmpty, setNewPassEmpty] = useState(false);
  const [newPassRepEmpty, setNewPassRepEmpty] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const changePasswordRequest = async (
    oldPassword: string,
    newPassword: string
  ) => {
    const res = await useAxiosAuth().post("/api/Authentication", {
      oldPassword: oldPassword,
      newPassword: newPassword,
    });
  };

  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
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
        setError("رمز عبور جدید با تکرار آن برابر نیست.");
      else {
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
        className="flex flex-col gap-2 mx-auto mt-10 max-w-md text-sm px-5 pt-5 pb-12 relative"
        onSubmit={handleSubmit}
      >
        <label className="mt-10 text-right">رمز عبور قبلی</label>
        <input
          name="oldPassword"
          type="password"
          onClick={() => setError(null)}
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
          onClick={() => setError(null)}
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
          onClick={() => setError(null)}
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
        <p className="text-red-600 text-lg mb-6 absolute -bottom-0.5">
          {error}
        </p>
      </form>
    </div>
  );
};

export default Profile;

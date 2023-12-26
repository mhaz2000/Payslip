"use client";

import "react-toastify/ReactToastify.css";
import { signIn } from "next-auth/react";
import { useRouter } from "next/navigation";
import { FormEvent, useState } from "react";
import { displayError } from "@/lib/toastDisplay";


const LoginForm = () => {
  const router = useRouter();
  const [usernameEmpty, setUsernameEmpty] = useState(false);
  const [passwordEmpty, setPasswordEmpty] = useState(false);

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formData = new FormData(e.currentTarget);
    const username = formData.get("username");
    const password = formData.get("password");

    if (
      password &&
      username &&
      (password.valueOf() as string).length &&
      (username.valueOf() as string).length
    ) {
      const res = await signIn("credentials", {
        username: username,
        password: password,
        redirect: false,
        callbackUrl: "/register",
      });

      if (res?.ok) {
        router.push("/dashboard");
        router.refresh();
      } else if (res?.status === 401 || res?.status === 403) {
        displayError("نام کاربری یا رمز عبور اشتباه است.");
      }
    } else {
      if (!username || (username.valueOf() as string).trim().length === 0)
        setUsernameEmpty(true);

      if (!password || (password.valueOf() as string).trim().length === 0)
        setPasswordEmpty(true);
    }
  };

  return (
    <form
      className="flex flex-col gap-2 mx-auto mt10 max-w-md"
      onSubmit={handleSubmit}
    >
      <label className="mt-10 text-xs text-right">نام کاربری</label>
      <input
        name="username"
        type="username"
        onChange={(e) => {
          if (e.target.value.length > 0) setUsernameEmpty(false);
        }}
        className={`border border-black text-black  rounded-md h-8 w-64 p-2
        ${usernameEmpty ? "bg-red-400" : "bg-slate-200"}`}
      />
      <label className="text-xs text-right mt-3">رمز عبور</label>
      <input
        name="password"
        type="password"
        onChange={(e) => {
          if (e.target.value.length > 0) setPasswordEmpty(false);
        }}
        className={`border border-black text-black rounded-md h-8 w-64 p-2 ${
          passwordEmpty ? " bg-red-400" : "bg-slate-200"
        }`}
      />
      <button
        type="submit"
        className="border rounded-lg h-9 w-44 mx-auto my-5 hover:bg-gray-300 hover:border-slate-900 hover:text-slate-900 ease-in duration-300"
      >
        ورود
      </button>
    </form>
  );
};

export default LoginForm;

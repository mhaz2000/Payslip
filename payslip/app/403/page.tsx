import Link from "next/link";
import { GoAlert } from "react-icons/go";

const AccessDenied = () => {
  return (
    <div className="flex flex-col justify-center items-center w-full h-1/2 gap-5">
      <span className="text-9xl"><GoAlert /></span>
      <h1 className="text-4xl">شما دسترسی لازم برای مشاهده محتوای این صفحه ندارید.</h1>
      <Link className="btn-style" href="/dashboard">بازگشت به پنل کاربری</Link>
    </div>
  );
};

export default AccessDenied;

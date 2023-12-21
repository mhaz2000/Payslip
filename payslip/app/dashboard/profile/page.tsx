"use client";
const Profile = () => {
  const handleSubmit = () => {};
  return (
    <div className="bg-cyan-950 lg:w-1/3 md:w-1/2 mx-auto w-5/6 rounded-xl shadow-2xl shadow-black">
      <form
        className="flex flex-col gap-2 mx-auto mt-10 max-w-md text-sm px-5"
        onSubmit={handleSubmit}
      >
        <label className="mt-10 text-right">رمز عبور قبلی</label>
        <input
          name="password"
          type="password"
          className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto`}
        />
        <label className="text-right mt-3">رمز عبور جدید</label>
        <input
          name="password"
          type="password"
          className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto`}
        />
        <label className="text-right mt-3">تکرار عبور جدید</label>
        <input
          name="password"
          type="password"
          className={`border border-black text-black rounded-md h-8 w-full p-2 mx-auto`}
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

"use client";

import { signOut, useSession } from "next-auth/react";
import { FaUser } from "react-icons/fa";

const Navbar = () => {
  const { data: session } = useSession();
  return (
    <nav className="flex flex-row w-full justify-between px-10 py-5">
      <div className="flex flex-row gap-3 items-center">
      <span className="border rounded-full p-1">
            <FaUser />
          </span>
        <h2 className="">{session?.user.fullName}</h2>
      </div>
      <button className="border-2 rounded-xl py-1 px-5
       hover:bg-gray-300 hover:text-slate-900 ease-in duration-300" onClick={()=> signOut()}>خروج</button>
    </nav>
  );
};

export default Navbar;

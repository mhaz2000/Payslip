"use client";

import { signOut, useSession } from "next-auth/react";
import Link from "next/link";
import { useState } from "react";
import { FaUser } from "react-icons/fa";

const Navbar = () => {
  const { data: session } = useSession();
  const [onAdminPanel, setOnAdminPanel] = useState(false);
  return (
    <nav className="flex flex-row w-full justify-between px-10 py-5">
      <div className="flex flex-row gap-3 items-center">
        <span className="border rounded-full p-1">
          <FaUser />
        </span>
        <h2 className="">{session?.user.fullName}</h2>
      </div>
      <div className="flex flex-row gap-3">
        {session?.user.isAdmin && (
          <Link
            href={onAdminPanel ? "/dashboard" : "/admin"}
            className="btn-style"
            onClick={() => setOnAdminPanel((prev) => !prev)}
          >
            {onAdminPanel ? "پنل کاربری" : "پنل ادمین"}
          </Link>
        )}
        <button
          className="btn-style"
          onClick={() => signOut()}
        >
          خروج
        </button>
      </div>
    </nav>
  );
};

export default Navbar;

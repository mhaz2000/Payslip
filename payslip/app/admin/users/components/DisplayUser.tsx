"use client";

import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { displayError, displaySuccess } from "@/lib/toastDisplay";
import { AxiosResponse } from "axios";
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import ChangeUserPassword from "./ChangeUserPassword";

interface DisplayUserComponentProps {
  handleCount: (count: number) => void;
  current: number;
  search: string;
  gridRefresh: boolean;
}

interface Response {
  total: number;
  data: UserModel[];
}

const DisplayUser: React.FC<DisplayUserComponentProps> = ({
  handleCount,
  current,
  search,
  gridRefresh,
}) => {
  useSession();

  const [users, setUsers] = useState<UserModel[]>([]);
  const [refresh, setRefresh] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const axiosAuth = useAxiosAuth();

  const removeUser = async (userId: string) => {
    try {
      await axiosAuth.delete(`/api/users/${userId}`);

      displaySuccess("کاربر با موفقیت حذف شد.");
      setRefresh((prev) => (prev = !prev));
    } catch (error: any) {
      displayError(error.data.message);
    }
  };

  const changePassword = async (userId: string) => {
    try {
      await axiosAuth.delete(`/api/users/${userId}`);

      displaySuccess("کاربر با موفقیت حذف شد.");
      setRefresh((prev) => (prev = !prev));
    } catch (error: any) {
      displayError(error.data.message);
    }
  };

  const toggleActivation = async (userId: string) => {
    try {
      await axiosAuth.put(`/api/users/toggleActivation/${userId}`);

      displaySuccess("وضعیت کاربر با موفقیت تغییر یافت.");
      setRefresh((prev) => (prev = !prev));
    } catch (error: any) {
      displayError(error.data.message);
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response: AxiosResponse<Response> = await axiosAuth.get(
          `/api/users?skip=${(current - 1) * 10}&search=${search}`
        );
        setUsers(response.data.data);
        handleCount(response.data.total);
      } catch (error: any) {
        displayError(error.data.message);
      }
      setIsLoading(false);
    };
    setIsLoading(true);
    setTimeout(() => {
      fetchData();
    }, 200);
  }, [current, search, refresh, gridRefresh]);
  return (
    <>
      {isLoading ? (
        <p className="my-5 text-slate-400 mr-2">در حال دریافت اطلاعات</p>
      ) : (
        <tbody className="bg-table">
          {users.map((user, index) => (
            <tr
              className={`bg-black ${index % 2 === 0 ? "bg-opacity-20" : ""}`}
              key={user.id}
            >
              <td className="pr-7">{index + 1}</td>
              <td className="flex px-6 py-4 whitespace-nowrap">
                <span className="ml-2 font-medium">{user.firstName}</span>
              </td>
              <td className="px-6 py-4 whitespace-nowrap">{user.lastName}</td>
              <td className="px-6 py-4 whitespace-nowrap">
                {user.nationalCode}
              </td>
              <td className="px-6 py-4 whitespace-nowrap">{user.cardNumber}</td>
              <td className="px-6 py-4 whitespace-nowrap">
                <span className="flex flex-row gap-2">
                  <ChangeUserPassword userId={user.id} />
                  <button
                    className="btn-style w-24 bg-black border-red-500 text-red-500 hover:bg-red-500"
                    onClick={() => removeUser(user.id)}
                  >
                    حذف
                  </button>
                  <button
                    onClick={() => toggleActivation(user.id)}
                    className={`btn-style w-24 bg-black ${
                      user.isActive
                        ? "border-yellow-500 text-yellow-500 hover:bg-yellow-500"
                        : "border-green-500 text-green-500 hover:bg-green-500"
                    } `}
                  >
                    {user.isActive ? "غیر فعال" : "فعال"}
                  </button>
                </span>
              </td>
            </tr>
          ))}
        </tbody>
      )}
    </>
  );
};

export default DisplayUser;

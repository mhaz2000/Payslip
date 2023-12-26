"use client";

import useAxiosAuth from "@/lib/hooks/useAxiosAuth";
import { displayError } from "@/lib/toastDisplay";
import { AxiosResponse } from "axios";
import { useEffect, useState } from "react";

interface DisplayUserComponentProps {
  handleCount: (count: number) => void;
  current: number;
  search: string;
}

interface Response {
  total: number;
  data: UserModel[];
}

const DisplayUser: React.FC<DisplayUserComponentProps> = ({
  handleCount,
  current,
  search,
}) => {
  const [users, setUsers] = useState<UserModel[]>([]);
  const axiosAuth = useAxiosAuth();

  useEffect(() => {
    const fetchData = async () => {
      const response: AxiosResponse<Response> = await axiosAuth.get(
        `/api/users?skip=${(current - 1) * 10}&search=${search}`
      );
      setUsers(response.data.data);
      handleCount(response.data.total);
      try {
      } catch (error: any) {
        displayError(error.data.message);
      }
    };
    setTimeout(() => {
      fetchData();
    }, 200);
  }, [current, search]);
  return (
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
          <td className="px-6 py-4 whitespace-nowrap">{user.nationalCode}</td>
          <td className="px-6 py-4 whitespace-nowrap">{user.cardNumber}</td>
          <td className="px-6 py-4 whitespace-nowrap">
            <div className="flex flex-row gap-1">
              <button className="btn-style bg-black border-red-500 text-red-500 hover:bg-red-500">
                حذف
              </button>
              <button className="btn-style bg-black border-yellow-500 text-yellow-500 hover:bg-yellow-500">
                غیر فعال
              </button>
            </div>
          </td>
        </tr>
      ))}
    </tbody>
  );
};

export default DisplayUser;

"use client";

import { useState } from "react";
import TablePagination from "../../components/TablePagination";
import UserList from "./components/UserList";
import UserSearch from "./components/UserSearch";
import CreateUser from "./components/CreateUser";

const Users = () => {
  const [current, setCurrent] = useState(1);
  const [count, setCount] = useState(0);
  const [search, setSearch] = useState("");
  const [gridRefresh, setGridRefresh] = useState(true);

  const handleCount = (count: number) => {
    setCount(Math.ceil(count / 10));
  };

  const handlePageChange = (current: number) => {
    setCurrent(current);
  };

  const handleSearch = (search: string) => {
    setSearch(search);
    setGridRefresh((prev) => !prev);
  };

  const handleCreateUser = () => {
    setGridRefresh((prev) => !prev);
  }

  return (
    <div className="pt-5">
      <div className="flex flex-row w-full px-10">
        <CreateUser handleCreateUser={handleCreateUser} />
        <UserSearch handleSearch={handleSearch} />
      </div>
      <UserList
        handleCount={handleCount}
        current={current}
        search={search}
        gridRefresh={gridRefresh}
      />
      <TablePagination
        pageChangeHandler={handlePageChange}
        count={count}
        current={current}
      />
    </div>
  );
};

export default Users;

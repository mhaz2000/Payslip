"use client";

import { useState } from "react";
import TablePagination from "./TablePagination";
import UserList from "./UserList";
import UserSearch from "./UserSearch";

const Users = () => {
  const [current, setCurrent] = useState(1);
  const [count, setCount] = useState(0);
  const [search, setSearch] = useState("");

  const handleCount = (count: number) => {
    setCount(Math.ceil(count / 10));
  };

  const handlePageChange = (current: number) => {
    setCurrent(current);
  };

  const handleSearch = (search: string) => {
    setSearch(search);
  };

  return (
    <div className="pt-5">
      <div className="flex flex-row w-full">
        
        <UserSearch handleSearch={handleSearch} />
      </div>
      <UserList handleCount={handleCount} current={current} search={search} />
      <TablePagination
        pageChangeHandler={handlePageChange}
        count={count}
        current={current}
      />
    </div>
  );
};

export default Users;

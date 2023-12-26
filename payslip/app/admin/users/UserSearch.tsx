"use client";

import { useState } from "react";

interface UserSearchProps {
  handleSearch: (search: string) => void;
}

const UserSearch: React.FC<UserSearchProps> = ({ handleSearch }) => {
  const [search, setSearch] = useState("");
  return (
    <div className="flex flex-row-reverse justify-start gap-4 px-10 w-full">
      <input
        className="rounded-xl text-black px-3 xl:w-1/6 lg:w-1/4 md:w-1/3 w-full"
        onChange={(e) => setSearch(e.target.value)}
        type="text"
        value={search}
      />
      <button className="btn-style" onClick={()=> handleSearch(search)}>جستجو</button>
    </div>
  );
};

export default UserSearch;

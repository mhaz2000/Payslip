"use client";
import { useState } from "react";

const DropDown = ({ title }: { title: string }) => {
  const [toggle, setToggle] = useState(false);
  const [displayTitle, setDisplayTitle] = useState<string|null>(title)

  return (
    <div className="flex-none p-2">
      <button
        onClick={() => setToggle(!toggle)}
        className="flex flex-row-reverse justify-between w-48 px-2 py-2 text-gray-700 bg-white border-2 border-white rounded-md shadow focus:outline-none focus:border-blue-600"
      >
        <span className="select-none dir-right">{displayTitle}</span>

        <svg
          id="arrow-up"
          className={`${toggle ? "" : "hidden"}  w-6 h-6 stroke-current`}
          viewBox="0 0 20 20"
        >
          <path
            fillRule="evenodd"
            d="M14.707 12.707a1 1 0 01-1.414 0L10 9.414l-3.293 3.293a1 1 0 01-1.414-1.414l4-4a1 1 0 011.414 0l4 4a1 1 0 010 1.414z"
            clipRule="evenodd"
          />
        </svg>
        <svg
          id="arrow-down"
          className={`${!toggle ? "" : "hidden"} w-6 h-6 stroke-current`}
          viewBox="0 0 20 20"
        >
          <path
            fillRule="evenodd"
            d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
            clipRule="evenodd"
          />
        </svg>
      </button>
      <div
        id="options"
        className={`${
          toggle ? "" : "hidden"
        } w-48 py-2 mt-2 bg-white rounded-lg shadow-xl`}
      >
        <a
          href="#"
          className="block px-4 py-2 text-gray-800 hover:bg-indigo-500 hover:text-white"
          onClick={(e)=> setDisplayTitle('item1')}
        >
          Item 1
        </a>
        <a
          href="#"
          className="block px-4 py-2 text-gray-800 hover:bg-indigo-500 hover:text-white"
        >
          Item 2
        </a>
        <a
          href="#"
          className="block px-4 py-2 text-gray-800 hover:bg-indigo-500 hover:text-white"
        >
          Item 3
        </a>
      </div>
    </div>
  );
};

export default DropDown;

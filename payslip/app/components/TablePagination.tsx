"use client";
import { useEffect, useState } from "react";
import { FaArrowRightLong } from "react-icons/fa6";
import { FaArrowLeftLong } from "react-icons/fa6";

interface TablePaginationProps {
  pageChangeHandler: (current: number) => void;
  count: number;
  current: number;
}

const TablePagination: React.FC<TablePaginationProps> = ({
  pageChangeHandler,
  count,
  current,
}) => {
  const [numbers, setNumbers] = useState<number[]>([]);

  useEffect(() => {
    // Update the numbers array whenever count changes
    setNumbers(Array.from(Array(count).keys()));
  }, [count]);

  const handlePageChange = (page: number) => {
    pageChangeHandler(page);
  };

  const renderPageNumbers = () => {
    const totalPages = numbers.length;
    const displayPages = 5; // Display only 5 pages

    if (totalPages <= displayPages) {
      return numbers.map((page) => (
        <p
          key={page}
          className={`text-sm font-medium leading-none cursor-pointer ${
            page + 1 === current ? "text-white" : "text-gray-400"
          } border-t border-transparent hover:border-gray-300 pt-3 mr-4 px-2`}
          onClick={() => handlePageChange(page + 1)}
        >
          {page + 1}
        </p>
      ));
    }

    const middlePage = Math.floor(displayPages / 2);

    let startPage = Math.max(0, current - middlePage);
    let endPage = Math.min(totalPages, startPage + displayPages);

    // Ensure only 5 pages are displayed, and adjust if the current page is close to the edges
    if (endPage - startPage < displayPages) {
      startPage = Math.max(0, totalPages - displayPages);
    } else if (current <= middlePage) {
      endPage = displayPages;
    } else if (current >= totalPages - middlePage) {
      startPage = totalPages - displayPages;
    }

    return (
      <>
        {startPage > 0 && (
          <>
            <p
              className="text-sm font-medium leading-none cursor-pointer text-gray-300 border-t border-transparent pt-3 mr-4 px-2"
              onClick={() => handlePageChange(1)}
            >
              1
            </p>
            {startPage > 1 && (
              <p className="text-sm font-medium leading-none cursor-pointer text-gray-300 pt-3 mr-4 px-2">
                ...
              </p>
            )}
          </>
        )}

        {numbers.slice(startPage, endPage).map((page) => (
          <p
            key={page}
            className={`text-sm font-medium leading-none cursor-pointer ${
              page + 1 === current ? "text-white" : "text-gray-400"
            } border-t border-transparent hover:border-gray-300 pt-3 mr-4 px-2`}
            onClick={() => handlePageChange(page + 1)}
          >
            {page + 1}
          </p>
        ))}

        {endPage < totalPages && (
          <>
            {endPage < totalPages - 1 && (
              <p className="text-sm font-medium leading-none cursor-pointer text-gray-300 pt-3 mr-4 px-2">
                ...
              </p>
            )}
            <p
              className="text-sm font-medium leading-none cursor-pointer text-gray-300 border-t border-transparent pt-3 mr-4 px-2"
              onClick={() => handlePageChange(totalPages)}
            >
              {totalPages}
            </p>
          </>
        )}
      </>
    );
  };

  return (
    <div className="flex items-center justify-center py-5 px-10">
      <div className="w-full xl:w-3/4 lg:w-1/2 flex items-center justify-between border-pagination text-gray-300 border-t-2">
        <div
          className="flex items-center pt-3 text-gray-300 hover:text-slate-50 cursor-pointer"
          onClick={() => {
            if (current != 1) handlePageChange(current - 1);
          }}
        >
          <p className="text-lg ml-3 font-medium leading-none">
            <FaArrowRightLong />
          </p>
        </div>
        <div className="flex">{renderPageNumbers()}</div>
        <div
          className="flex items-center pt-3 text-gray-300 hover:text-slate-50 cursor-pointer"
          onClick={() => {
            if (current != count) handlePageChange(current + 1);
          }}
        >
          <p className="text-lg font-medium leading-none mr-3">
            <FaArrowLeftLong />
          </p>
        </div>
      </div>
    </div>
  );
};

export default TablePagination;

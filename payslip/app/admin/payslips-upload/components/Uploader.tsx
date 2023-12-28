"use client";
import { useState } from "react";
import { BsCloudUploadFill } from "react-icons/bs";
import { FaFileAlt } from "react-icons/fa";

const Uploader = () => {
  const [selectedFile, setSelectedFile] = useState<any>(null);
  const [checkFile, setCheckFile] = useState(false);

  const imageHandler = (e: any) => {
    setSelectedFile(e.target.files[0]);
    setCheckFile(true);
  };

  return (
    <>
      <div className="flex gap-2 justify-center">
        <div className="h-24 w-52 cursor-pointer relative flex justify-center items-center border-2 rounded-md bg-gray-200">
          <input
            accept=".xls,.xlsx"
            type="file"
            name="file"
            onChange={imageHandler}
            className="z-20 opacity-0 cursor-pointer h-full w-full"
          />
          <div
            className={`absolute flex justify-center items-center gap-2 w-full h-full ${
              selectedFile
                ? ""
                : "border-2 border-dashed rounded-md border-black"
            }`}
          >
            {
              selectedFile ? (
                <div className="text-black flex-col items-center justify-center gap-2">
                  <span className="flex justify-center text-2xl mb-1">
                    <FaFileAlt />
                  </span>
                  <p>{selectedFile.name}</p>
                </div>
              ) : (
                <div className="text-black flex-col items-center justify-center gap-1">
                  <span className="flex justify-center text-2xl">
                    <BsCloudUploadFill />
                  </span>
                  <p>انتخاب فایل</p>
                </div>
              )
              //   selectedFile.name
            }
          </div>
        </div>
      </div>
    </>
  );
};

export default Uploader;

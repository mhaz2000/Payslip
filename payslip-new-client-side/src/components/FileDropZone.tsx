import { Upload } from "lucide-react";
import React, { useCallback, useState } from "react";
import { useTranslation } from "react-i18next";

type FileDropZoneProps = {
  onSelect: (file: File) => void;
  accept?: string; // e.g. '.xlsx,.xls'
  multiple?: boolean;
  label?: string;
};

const FileDropZone: React.FC<FileDropZoneProps> = ({
  onSelect,
  accept = ".xlsx,.xls",
  multiple = false,
  label,
}) => {
  const { t } = useTranslation();

  const [isDragging, setIsDragging] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleDragOver = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(true);
  }, []);

  const handleDragLeave = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(false);
  }, []);

  const validateAndSelectFile = (file: File) => {
    const allowedExtensions = accept.split(",").map((s) => s.trim().toLowerCase());
    const fileExtension = file.name.slice(file.name.lastIndexOf(".")).toLowerCase();

    if (!allowedExtensions.includes(fileExtension)) {
      setError(t("onlyFileTypesAllowed", { types: accept }));
      return false;
    }

    setError(null);
    onSelect(file);
    return true;
  };

  const handleDrop = useCallback(
    (e: React.DragEvent) => {
      e.preventDefault();
      e.stopPropagation();
      setIsDragging(false);

      const files = e.dataTransfer.files;
      if (!files.length) return;

      // if multiple allowed, you might want to send array or just first file — here sending first file only
      validateAndSelectFile(files[0]);
    },
    [accept, onSelect, t]
  );

  const handleFileInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (!files || !files.length) return;

    validateAndSelectFile(files[0]);
  };

  return (
    <div>
      <label
        htmlFor="file-upload"
        className={`flex flex-col items-center justify-center p-6 border-2 rounded-lg cursor-pointer transition
          ${isDragging ? "border-blue-600 bg-blue-50" : "border-gray-300 bg-white"}
          hover:border-blue-400`}
        onDragOver={handleDragOver}
        onDragLeave={handleDragLeave}
        onDrop={handleDrop}
      >
        <input
          id="file-upload"
          type="file"
          accept={accept}
          multiple={multiple}
          className="hidden"
          onChange={handleFileInputChange}
        />
        <Upload className="m-6" />
        <p className="text-sm text-gray-600">{label || t("fileDropZoneLabel")}</p>
      </label>
      {error && <p className="text-xs text-red-600 mt-1">{error}</p>}
    </div>
  );
};

export default FileDropZone;

import { useState } from "react";
import { useTranslation } from "react-i18next";
import { Dialog } from "../../Dialog";
import FileDropZone from "../../FileDropZone";
import { importUsersFromExcel } from "../../../api/users";

interface ImportUsersDialogProps {
  open: boolean;
  onClose: () => void;
  onImported?: () => void;
}

export default function ImportUsersDialog({ open, onClose, onImported }: ImportUsersDialogProps) {
  const { t } = useTranslation();

  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleFileSelect = (file: File) => {
    setSelectedFile(file);
    setError(null);
  };

  const handleUpload = async () => {
    if (!selectedFile) {
      setError(t("pleaseSelectFile"));
      return;
    }
    setUploading(true);
    setError(null);
    try {
      await importUsersFromExcel(selectedFile);
      setSelectedFile(null);
      onImported?.();
      onClose();
    } catch (err) {
      setError(t("uploadFailed"));
      console.error("Upload error:", err);
    } finally {
      setUploading(false);
    }
  };

  return (
    <Dialog
      isOpen={open}
      onClose={() => {
        setSelectedFile(null);
        setError(null);
        onClose();
      }}
      title={t("importUsers")}
      content={
        <>
          <FileDropZone onSelect={handleFileSelect} accept=".xlsx,.xls" />
          {selectedFile && (
            <p className="mt-2 text-sm text-gray-700">
              {t("selectedFile")}: {selectedFile.name}
            </p>
          )}
          {error && <p className="mt-2 text-sm text-red-600">{error}</p>}
        </>
      }
      confirmText={uploading ? t("uploading") : t("upload")}
      cancelText={t("cancel")}
      onConfirm={handleUpload}
    />
  );
}

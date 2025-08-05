import { useState } from "react";
import { useTranslation } from "react-i18next";
import { Dialog } from "../../Dialog";
import FileDropZone from "../../FileDropZone";
import { uploadPayslip } from "../../../api/payslip";

interface UploadPayslipDialogProps {
    open: boolean;
    onClose: () => void;
    onUploaded?: () => void;
}

const months = [
    { value: 1, labelKey: "january" },
    { value: 2, labelKey: "february" },
    { value: 3, labelKey: "march" },
    { value: 4, labelKey: "april" },
    { value: 5, labelKey: "may" },
    { value: 6, labelKey: "june" },
    { value: 7, labelKey: "july" },
    { value: 8, labelKey: "august" },
    { value: 9, labelKey: "september" },
    { value: 10, labelKey: "october" },
    { value: 11, labelKey: "november" },
    { value: 12, labelKey: "december" },
];

export default function UploadPayslipDialog({ open, onClose, onUploaded }: UploadPayslipDialogProps) {
    const { t, i18n } = useTranslation();
    const isFa = i18n.language === "fa";

    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [month, setMonth] = useState<number | null>();
    const [year, setYear] = useState<number | null>();
    const [uploading, setUploading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    // Generate year list based on locale
    const currentYear = new Date().getFullYear();

    const yearRange = Array.from({ length: 5 }, (_, i) => {
        const gYear = currentYear - i + 1;

        if (isFa) {
            // Use Intl.DateTimeFormat with Persian calendar to format Jan 1 of Gregorian year `gYear`
            const persianYear = new Intl.DateTimeFormat("fa-IR-u-ca-persian", {
                year: "numeric",
            }).format(new Date(gYear, 0, 1)); // month 0 = January

            // The formatted year comes as Persian digits (e.g. '۱۴۰۴'), so convert to normal number:
            // Optionally, convert Persian digits to English digits:
            const persianDigits = "۰۱۲۳۴۵۶۷۸۹";
            const englishDigits = "0123456789";

            const normalizedYear = persianYear
                .split("")
                .map((ch) => {
                    const index = persianDigits.indexOf(ch);
                    return index >= 0 ? englishDigits[index] : ch;
                })
                .join("");

            return Number(normalizedYear);
        }

        return gYear;
    });

    const handleUpload = async () => {
        if (!selectedFile || !month || !year) {
            setError(t("pleaseCompleteAllFields"));
            return;
        }

        setUploading(true);
        setError(null);
        try {
            await uploadPayslip(selectedFile, month, year);
            onUploaded?.();
            setSelectedFile(null);
            setMonth(null);
            setYear(null);
            onClose();
        } catch (err) {
            console.error("Upload error:", err);
            setError(t("uploadFailed"));
        } finally {
            setUploading(false);
        }
    };

    return (
        <Dialog
            isOpen={open}
            onClose={() => {
                setSelectedFile(null);
                setMonth(null);
                setYear(null);
                setError(null);
                onClose();
            }}
            title={t("uploadPayslip")}
            content={
                <>
                    <div className="mb-4">
                        <label className="block text-sm font-medium text-gray-700">{t("month")}</label>
                        <select
                            value={month ?? ''}
                            onChange={(e) => setMonth(Number(e.target.value))}
                            className="mt-1 block w-full rounded border border-gray-300 p-2"
                        >
                            <option value="">{t("selectMonth")}</option>
                            {months.map((m) => (
                                <option key={m.value} value={m.value}>
                                    {t(m.labelKey)}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className="mb-4">
                        <label className="block text-sm font-medium text-gray-700">{t("year")}</label>
                        <select
                            value={year ?? ''}
                            onChange={(e) => setYear(Number(e.target.value))}
                            className="mt-1 block w-full rounded border border-gray-300 p-2"
                        >
                            <option value="">{t("selectYear")}</option>
                            {yearRange.map((y) => (
                                <option key={y} value={y}>{y}</option>
                            ))}
                        </select>
                    </div>

                    <FileDropZone onSelect={setSelectedFile} accept=".xlsx,.xls,.pdf" />
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

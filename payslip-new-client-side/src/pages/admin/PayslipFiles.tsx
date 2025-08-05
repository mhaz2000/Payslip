import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { Download, MoreVertical, Trash2 } from "lucide-react";
import { Dialog } from "../../components/Dialog";
import { useDebounce } from "../../hooks/useDebounce";
import Button from "../../components/ui/Button";
import type { PayslipFile } from "../../types/payslip";
import { getAllPayslips, removePayslip } from "../../api/payslip";
import UploadPayslipDialog from "../../components/admin/payslips/UploadPayslipDialog";
import { authorizedAxios } from "../../api/axios";
import { toast } from "react-toastify";
import { toPersianDigits } from "../../utils/persianDigits";

const PAGE_SIZE = 7;

export function PayslipFiles() {
    const { t, i18n } = useTranslation();
    const dir = i18n.dir();
    const textAlignClass = dir === "rtl" ? "text-right" : "text-left";

    const [payslips, setPayslips] = useState<PayslipFile[]>([]);
    const [page, setPage] = useState(1);
    const [search, setSearch] = useState("");
    const debouncedSearch = useDebounce(search, 500);
    const [loading, setLoading] = useState(false);
    const [totalCount, setTotalCount] = useState<number | undefined>(undefined);
    const [openMenuId, setOpenMenuId] = useState<string | null>(null); // to track which menu is open
    const [confirmOpen, setConfirmOpen] = useState(false);
    const [isImportOpen, setIsImportOpen] = useState(false);

    const [payslipToDelete, setPayslipToDelete] = useState<PayslipFile | null>(null);


    const downloadPayslipFile = async (
        payslipId: string,
        month: string,
        year: number
    ) => {
        let res = await authorizedAxios.get(`files/${payslipId}`, {
            responseType: "arraybuffer",
            headers: {
                "Content-Disposition": `attachment; filename=file.xlsx`,
                "Content-Type": "application/octet-stream",
            },
        });

        const blob = new Blob([res.data], { type: "application/octet-stream" });

        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = `${month}-${year}.xlsx`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    };


    const fetchPaylips = async () => {
        setLoading(true);
        try {
            const response = await getAllPayslips(page, debouncedSearch);
            setPayslips(response.data);
            setTotalCount(response.total);
        } catch (err: any) {
            toast.error(err.response.data.message || t("errorFetchingPayslips"));
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPaylips();
    }, [page, debouncedSearch]);

    const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSearch(e.target.value);
        setPage(1);
    };

    const totalPages = totalCount ? Math.ceil(totalCount / PAGE_SIZE) : undefined;

    // Toggle dropdown menu open/close
    const toggleMenu = (id: string) => {
        setOpenMenuId((curr) => (curr === id ? null : id));
    };

    const DeletePayslip = (payslip: PayslipFile) => {
        setOpenMenuId(null);
        setPayslipToDelete(payslip);
        setConfirmOpen(true);
    };

    const handleConfirmDeletePayslip = async () => {
        if (!payslipToDelete) return;

        try {
            await removePayslip(payslipToDelete.fileId);
            // refetch payslips to update the table after deletion
            await fetchPaylips();
            setConfirmOpen(false);
            setPayslipToDelete(null);
        } catch (err:any) {
            toast.error(err.response.data.message);
            // optionally show an error toast or alert here
        }
    };

    return (
        <>
            <div className="max-w-5xl mx-auto p-4 bg-background relative" dir={dir}>
                <div className={`flex flex-col sm:flex-row sm:items-center sm:justify-between gap-2 mb-4 ${dir === "rtl" ? "sm:flex-row-reverse" : ""}`}>
                    <input
                        type="text"
                        placeholder={t("searchPayslipsPlaceholder")}
                        value={search}
                        onChange={handleSearchChange}
                        className="w-full sm:w-[300px] p-2 border border-muted rounded-md focus:outline-none focus:ring-2 focus:ring-primary"
                    />
                    <Button
                        onClick={() => setIsImportOpen(true)}
                        className="bg-primary text-white px-4 py-2 rounded-md hover:bg-primary/90 transition"
                    >
                        {t("importPayslip")}
                    </Button>
                </div>


                {loading ? (
                    <div className="text-center py-10 text-muted">{t("loading")}</div>
                ) : (
                    <>
                        <div className="w-full overflow-x-auto rounded-md border border-muted">
                            <table className="min-w-full">
                                <thead className="bg-primary text-white">
                                    <tr>
                                        <th className={`py-3 px-6 ${textAlignClass}`}>#</th>
                                        <th className={`py-3 px-6 ${textAlignClass}`}>{t("importDate")}</th>
                                        <th className={`py-3 px-6 ${textAlignClass}`}>{t("year")}</th>
                                        <th className={`py-3 px-6 ${textAlignClass}`}>{t("month")}</th>
                                        <th className={`py-3 px-6 ${textAlignClass}`}>{t("actions")}</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {payslips.length === 0 ? (
                                        <tr>
                                            <td colSpan={6} className={`text-center py-4 text-muted italic ${textAlignClass}`}>
                                                {t("noPayslipFound")}
                                            </td>
                                        </tr>
                                    ) : (
                                        payslips.map((payslip, idx) => (
                                            <tr
                                                key={payslip.fileId}
                                                className="even:bg-accent hover:bg-secondary/20 transition-colors"
                                            >
                                                <td className={`py-3 px-6 ${textAlignClass}`}>
                                                    {(page - 1) * PAGE_SIZE + idx + 1}
                                                </td>
                                                <td className={`py-3 px-6 ${textAlignClass}`}>{payslip.uploadedDate}</td>
                                                <td className={`py-3 px-6 ${textAlignClass}`}>{toPersianDigits(payslip.year)}</td>
                                                <td className={`py-3 px-6 ${textAlignClass}`}>{payslip.month}</td>

                                                <td className={`py-3 px-6`}>
                                                    <button
                                                        onClick={() => toggleMenu(payslip.fileId)}
                                                        aria-label="Actions"
                                                        className="p-1 rounded hover:bg-primary/20"
                                                    >
                                                        <MoreVertical className="h-6 w-6 text-muted" />
                                                    </button>

                                                    {openMenuId === payslip.fileId && (
                                                        <div
                                                            className={`
                                                              absolute z-30 mt-2 min-w-[140px] bg-white rounded-xl shadow-lg
                                                              ring-1 ring-black ring-opacity-5 origin-top-right
                                                              ${dir === "rtl" ? "left-10 right-auto" : "right-0"}
                                                              animate-dropdown-fade-scale overflow-hidden
                                                            `}
                                                        >
                                                            <button
                                                                onClick={() => DeletePayslip(payslip)}
                                                                className="flex items-center gap-3 w-full px-5 py-3 text-sm font-medium hover:bg-accent/30 transition-colors"
                                                                type="button"
                                                            >
                                                                <Trash2 className="w-5 h-5 text-danger" />
                                                                {t("delete")}
                                                            </button>

                                                            <button
                                                                onClick={() => downloadPayslipFile(payslip.fileId, payslip.month, payslip.year)}
                                                                className="flex items-center gap-3 w-full px-5 py-3 text-sm font-medium hover:bg-accent/30 transition-colors"
                                                                type="button"
                                                            >
                                                                <Download className="w-5 h-5" />
                                                                {t("download")}
                                                            </button>
                                                        </div>

                                                    )}

                                                </td>

                                            </tr>
                                        ))
                                    )}
                                </tbody>
                            </table>
                        </div>

                        <div className={`flex justify-between items-center mt-4 ${dir === "rtl" ? "flex-row-reverse" : ""}`}>
                            <button
                                onClick={() => setPage((p) => Math.max(1, p - 1))}
                                disabled={page === 1}
                                className={`px-4 py-2 rounded-md border ${page === 1
                                    ? "border-muted text-muted cursor-not-allowed"
                                    : "border-primary text-primary hover:bg-primary/10"
                                    }`}
                            >
                                {t("previous")}
                            </button>
                            <span className="text-muted">
                                {t("page")} {page}
                                {totalPages ? ` ${t("of")} ${totalPages}` : ""}
                            </span>
                            <button
                                onClick={() =>
                                    setPage((p) => (totalPages ? Math.min(totalPages, p + 1) : p + 1))
                                }
                                disabled={totalPages ? page >= totalPages : false}
                                className={`px-4 py-2 rounded-md border ${totalPages && page >= totalPages
                                    ? "border-muted text-muted cursor-not-allowed"
                                    : "border-primary text-primary hover:bg-primary/10"
                                    }`}
                            >
                                {t("next")}
                            </button>
                        </div>
                    </>
                )}
            </div>
            <Dialog
                isOpen={confirmOpen}
                title={t("confirmDeleteTitle")}
                content={t("confirmDeleteContent")}
                onConfirm={handleConfirmDeletePayslip}
                onClose={() => setConfirmOpen(false)}
                confirmText={t("delete")}
                cancelText={t("cancel")}
            />

            <UploadPayslipDialog
                open={isImportOpen}
                onClose={() => setIsImportOpen(false)}
                onUploaded={() => {
                    fetchPaylips();
                    setIsImportOpen(false);
                }}
            />
        </>
    );
}

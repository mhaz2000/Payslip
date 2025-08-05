import React, { useEffect, useState } from "react";
import type { User } from "../../types/user";
import { toggleActivation, getAllUsers } from "../../api/users";
import { useTranslation } from "react-i18next";
import { CheckCircle, Edit, MoreVertical, UserCheck, UserX, XCircle } from "lucide-react";
import { Dialog } from "../../components/Dialog";
import { useDebounce } from "../../hooks/useDebounce";
import CreateUserDialog from "../../components/admin/users/CreateUserDialog";
import ImportUsersDialog from "../../components/admin/users/ImportUsersDialog";
import Button from "../../components/ui/Button";

const PAGE_SIZE = 7;

export function Users() {
    const { t, i18n } = useTranslation();
    const dir = i18n.dir();
    const textAlignClass = dir === "rtl" ? "text-right" : "text-left";

    const [users, setUsers] = useState<User[]>([]);
    const [page, setPage] = useState(1);
    const [search, setSearch] = useState("");
    const debouncedSearch = useDebounce(search, 500);
    const [loading, setLoading] = useState(false);
    const [totalCount, setTotalCount] = useState<number | undefined>(undefined);
    const [openMenuId, setOpenMenuId] = useState<string | null>(null); // to track which menu is open
    const [selectedUser, setSelectedUser] = useState<User | null>(null);
    const [isCreateOpen, setIsCreateOpen] = useState(false);
    const [confirmOpen, setConfirmOpen] = useState(false);
    const [isImportOpen, setIsImportOpen] = useState(false);

    const [userToToggleActivation, setUserToToggleActivation] = useState<User | null>(null);

    const fetchUsers = async () => {
        setLoading(true);
        try {
            const response = await getAllUsers(page, debouncedSearch);
            setUsers(response.data);
            setTotalCount(response.total);
        } catch (err) {
            console.error(t("errorFetchingUsers"), err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchUsers();
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

    const ToggleActivation = (user: User) => {
        setOpenMenuId(null);
        setUserToToggleActivation(user);
        setConfirmOpen(true);
    };

    const handleConfirmToggleActivation = async () => {
        if (!userToToggleActivation) return;

        try {
            await toggleActivation(userToToggleActivation.id);
            // refetch users to update the table after deletion
            await fetchUsers();
            setConfirmOpen(false);
            setUserToToggleActivation(null);
        } catch (error) {
            console.error("Failed to toggle activation user:", error);
            // optionally show an error toast or alert here
        }
    };

    return (
        <>
            <div className="max-w-5xl mx-auto p-4 bg-background relative" dir={dir}>
                <div className={`flex flex-col sm:flex-row sm:items-center sm:justify-between gap-2 mb-4 ${dir === "rtl" ? "sm:flex-row-reverse" : ""}`}>
                    <input
                        type="text"
                        placeholder={t("searchUsersPlaceholder")}
                        value={search}
                        onChange={handleSearchChange}
                        className="w-full sm:w-[300px] p-2 border border-muted rounded-md focus:outline-none focus:ring-2 focus:ring-primary"
                    />
                    <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-2">
                        <Button
                            onClick={() => setIsCreateOpen(true)}
                            className="bg-primary text-white px-4 py-2 rounded-md hover:bg-primary/90 transition"
                        >
                            {t("createUser")}
                        </Button>

                        <Button
                            onClick={() => setIsImportOpen(true)}
                            className="bg-primary text-white px-4 py-2 rounded-md hover:bg-primary/90 transition"
                        >
                            {t("importUsers")}
                        </Button>
                    </div>
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
                                        <th className={`py-3 px-6 ${textAlignClass}`}>{t("cardNumber")}</th>
                                        <th className={`py-3 px-6 ${textAlignClass}`}>{t("nationalCode")}</th>
                                        <th className={`py-3 px-6 ${textAlignClass}`}>{t("firstName")}</th>
                                        <th className={`py-3 px-6 ${textAlignClass}`}>{t("lastName")}</th>
                                        <th className={`py-3 px-6 ${textAlignClass}`}>{t("status")}</th>
                                        <th className={`py-3 px-6 ${textAlignClass}`}>{t("actions")}</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {users.length === 0 ? (
                                        <tr>
                                            <td colSpan={6} className={`text-center py-4 text-muted italic ${textAlignClass}`}>
                                                {t("noUsersFound")}
                                            </td>
                                        </tr>
                                    ) : (
                                        users.map((user, idx) => (
                                            <tr
                                                key={user.id}
                                                className="even:bg-accent hover:bg-secondary/20 transition-colors"
                                            >
                                                <td className={`py-3 px-6 ${textAlignClass}`}>
                                                    {(page - 1) * PAGE_SIZE + idx + 1}
                                                </td>
                                                <td className={`py-3 px-6 ${textAlignClass}`}>{user.cardNumber}</td>
                                                <td className={`py-3 px-6 ${textAlignClass}`}>{user.nationalCode}</td>
                                                <td className={`py-3 px-6 ${textAlignClass}`}>{user.firstName}</td>
                                                <td className={`py-3 px-6 ${textAlignClass}`}>{user.lastName}</td>
                                                <td className={`py-3 px-6 ${textAlignClass}`}>
                                                    {user.isActive ? (
                                                        <div className="flex items-center gap-2 text-green-600">
                                                            <CheckCircle className="w-5 h-5" />
                                                            <span className="hidden md:inline">{t("active")}</span>
                                                        </div>
                                                    ) : (
                                                        <div className="flex items-center gap-2 text-red-600">
                                                            <XCircle className="w-5 h-5" />
                                                            <span className="hidden md:inline">{t("deactive")}</span>
                                                        </div>
                                                    )}
                                                </td>
                                                <td className={`py-3 px-6`}>
                                                    <button
                                                        onClick={() => toggleMenu(user.id)}
                                                        aria-label="Actions"
                                                        className="p-1 rounded hover:bg-primary/20"
                                                    >
                                                        <MoreVertical className="h-6 w-6 text-muted" />
                                                    </button>

                                                    {openMenuId === user.id && (
                                                        <div
                                                            className={`
                                                              absolute z-50 mt-2 min-w-[140px] bg-white rounded-xl shadow-lg
                                                              ring-1 ring-black ring-opacity-5 origin-top-right
                                                              ${dir === "rtl" ? "left-0 right-auto" : "right-0"}
                                                              animate-dropdown-fade-scale overflow-hidden
                                                            `}
                                                        >
                                                            <button
                                                                onClick={() => {
                                                                    setSelectedUser(user);
                                                                    setIsCreateOpen(true);
                                                                    setOpenMenuId(null);
                                                                }}
                                                                className="flex items-center gap-3 w-full px-5 py-3 text-sm font-medium hover:bg-accent/30 transition-colors"
                                                                type="button"
                                                            >
                                                                <Edit className="w-5 h-5" />
                                                                {t("edit")}
                                                            </button>

                                                            <button
                                                                onClick={() => ToggleActivation(user)}
                                                                className={`
      flex items-center gap-3 w-full px-5 py-3
      ${user.isActive ? "text-danger" : "text-primary"} text-sm font-medium
      hover:${user.isActive ? "bg-danger/10" : "bg-primary/10"}
      transition-colors
    `}
                                                                type="button"
                                                            >
                                                                {user.isActive ? (
                                                                    <>
                                                                        <UserX className="w-5 h-5" />
                                                                        {t("deactive")}
                                                                    </>
                                                                ) : (
                                                                    <>
                                                                        <UserCheck className="w-5 h-5" />
                                                                        {t("active")}
                                                                    </>
                                                                )}
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
                title={userToToggleActivation?.isActive ? t("confirmDeactiveTitle") : t("confirmAactiveTitle")}
                content={userToToggleActivation?.isActive ? t("confirmDeactiveContent") : t("confirmActiveContent")}
                onConfirm={handleConfirmToggleActivation}
                onClose={() => setConfirmOpen(false)}
                confirmText={userToToggleActivation?.isActive ? t("deactive") : t("active")}
                cancelText={t("cancel")}
            />

            <CreateUserDialog
                open={isCreateOpen}
                user={selectedUser} // pass selected user
                onClose={() => {
                    setIsCreateOpen(false);
                    setSelectedUser(null);
                }}
                onCreated={() => {
                    fetchUsers();
                    setIsCreateOpen(false);
                    setSelectedUser(null);
                }}
            />

            <ImportUsersDialog
                open={isImportOpen}
                onClose={() => setIsImportOpen(false)}
                onImported={() => {
                    fetchUsers();
                    setIsImportOpen(false);
                }}
            />
        </>
    );
}

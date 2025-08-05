import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import type { User, UserEntry } from "../../../types/user";
import { createUser, updateUser } from "../../../api/users";
import { Dialog } from "../../Dialog";
import Input from "../../ui/Input";
import { toast } from "react-toastify";

interface CreateUserDialogProps {
    open: boolean;
    onClose: () => void;
    onCreated?: () => void;
    user?: User | null;
}

export default function CreateUserDialog({ open, onClose, onCreated, user }: CreateUserDialogProps) {
    const { register, handleSubmit, formState: { errors }, reset } = useForm<UserEntry>();
    const { t } = useTranslation();

    useEffect(() => {
        if (user) {
            reset(user); // pre-fill form for update
        } else {
            reset(); // reset for create
        }
    }, [user, reset, open]);

    const onSubmit = async (data: UserEntry) => {
        try {
            if (user) {
                await updateUser(data, user.id); // assuming id is present in user
            } else {
                await createUser(data);
            }

            onCreated?.();
            onClose();
            reset();
        } catch (error:any) {
            toast.error(error.response.data.message)
        }
    };

    const form = (
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <Input
                label={t("firstName")}
                placeholder={t("firstNamePlaceholder")}
                error={errors.firstName?.message}
                autoFocus
                {...register("firstName", { required: t("firstNameRequired") })}
            />

            <Input
                label={t("lastName")}
                placeholder={t("lastNamePlaceholder")}
                error={errors.lastName?.message}
                {...register("lastName", { required: t("lastNameRequired") })}
            />

            <Input
                label={t("nationalCode")}
                placeholder={t("nationalCodePlaceholder")}
                error={errors.nationalCode?.message}
                {...register("nationalCode", { required: t("nationalCodeRequired") })}
            />

            <Input
                label={t("cardNumber")}
                placeholder={t("cardNumberPlaceholder")}
                error={errors.cardNumber?.message}
                {...register("cardNumber", { required: t("cardNumberRequired") })}
            />

            {user && (
                <Input
                    label={t("password")}
                    placeholder={t("passwordPlaceholder")}
                    error={errors.password?.message}
                    type="password"
                    {...register("password", { required: t("passwordRequired") })}
                />
            )}
        </form>
    );

    return (
        <Dialog
            isOpen={open}
            onClose={() => {
                reset();
                onClose();
            }}
            title={user ? t("editUser") : t("createUser")}
            content={form}
            confirmText={user ? t("update") : t("create")}
            cancelText={t("cancel")}
            onConfirm={handleSubmit(onSubmit)}
        />
    );
}
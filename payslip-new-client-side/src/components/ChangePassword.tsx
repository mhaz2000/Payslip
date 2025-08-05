import { useForm } from 'react-hook-form';
import { useState } from 'react';
import * as z from 'zod';
import { useTranslation } from 'react-i18next';
import { updatePassword } from '../api/auth';
import Input from './ui/Input';
import Button from './ui/Button';
import { zodResolver } from "@hookform/resolvers/zod";

export default function ChangePassword() {
  const { t } = useTranslation();

  // Schema must be declared *after* t is available
  const schema = z
    .object({
      currentPassword: z.string().min(1, t('currentPasswordRequired')),
      newPassword: z.string().min(6, t('newPasswordMin')),
      repeatPassword: z.string().min(1, t('repeatPasswordRequired')),
    })
    .refine((data) => data.newPassword === data.repeatPassword, {
      message: t('passwordsDoNotMatch'),
      path: ['repeatPassword'],
    });

  type FormValues = z.infer<typeof schema>;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<FormValues>({
    resolver: zodResolver(schema),
  });

  const [serverError, setServerError] = useState('');

  const onSubmit = async (data: FormValues) => {
    try {
      setServerError('');
      await updatePassword({
        oldPassword: data.currentPassword,
        newPassword: data.newPassword,
      });
      alert(t('passwordUpdatedSuccessfully'));
    } catch (err:any) {
      setServerError(err.response.data.message || t('updatePasswordFailed'));
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="max-w-md mx-auto mt-10 space-y-4">
      <h2 className="text-xl font-semibold">{t('changePassword')}</h2>

      <div>
        <Input label={t('currentPassword')} type="password" {...register('currentPassword')} />
        {errors.currentPassword && <p className="text-red-500 text-sm">{errors.currentPassword.message}</p>}
      </div>

      <div>
        <Input label={t('newPassword')} type="password" {...register('newPassword')} />
        {errors.newPassword && <p className="text-red-500 text-sm">{errors.newPassword.message}</p>}
      </div>

      <div>
        <Input label={t('repeatPassword')} type="password" {...register('repeatPassword')} />
        {errors.repeatPassword && <p className="text-red-500 text-sm">{errors.repeatPassword.message}</p>}
      </div>

      {serverError && <p className="text-red-500 text-sm">{serverError}</p>}

      <Button className='bg-primary w-full text-background hover:outline-double hover:bg-primary/70' type="submit" disabled={isSubmitting}>
        {isSubmitting ? t('updating') : t('update')}
      </Button>
    </form>
  );
}
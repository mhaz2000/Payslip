import { useForm } from 'react-hook-form';
import { useAuth } from '../hooks/useAuth';
import Input from '../components/ui/Input';
import Button from '../components/ui/Button';
import type { LoginRequest } from '../types/auth';
import { useTranslation } from 'react-i18next';
import LanguageSwitcher from '../components/LanguageSwitcher';

const Login = () => {
  const { t } = useTranslation();

  const { register, handleSubmit, formState: { errors } } = useForm<LoginRequest>();
  const { login, isPending, error } = useAuth();

  const onSubmit = async (data: LoginRequest) => {
    await login(data, 'user');
  };

  return (
    <div
      className="
        min-h-screen flex items-center justify-center
        bg-no-repeat bg-cover bg-center
        bg-[url('/login-background-vertical.png')]
        sm:bg-[url('/login-background.jpg')]"
    >
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="w-full max-w-sm bg-primary p-8 rounded-lg shadow space-y-6"
        noValidate
      >
        <h2 className="text-3xl font-semibold text-center text-background">{t('login')}</h2>

        {error && <p className="text-danger text-center text-sm">{t(error)}</p>}

        <Input
          className="text-primary"
          label={t('username')}
          labelColor="text-background"
          type="username"
          placeholder={t('username')}
          {...register('username', { required: t('username') + ' ' + t('isRequired') + '.' })}
          error={errors.username?.message}
        />

        <Input
          className="text-primary"
          label={t('password')}
          labelColor="text-background"
          type="password"
          placeholder={t('password')}
          {...register('password', { required: t('password') + ' ' + t('isRequired') + '.' })}
          error={errors.password?.message}
        />

        <Button className='w-full outline-double bg-background text-primary hover:text-background hover:bg-primary' type="submit" isLoading={isPending}>
          {t('login')}
        </Button>

        <div className='pt-8 flex justify-center'>
          <LanguageSwitcher />
        </div>
      </form>
    </div>
  );
};

export default Login;

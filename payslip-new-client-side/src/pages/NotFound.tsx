import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

export default function NotFound() {
  const { t } = useTranslation();

  return (
    <div className="min-h-screen bg-background flex items-center justify-center px-4">
      <div className="max-w-md w-full bg-white shadow-lg rounded-xl p-8 text-center space-y-6">
        <h1 className="text-6xl font-bold text-primary">404</h1>
        <p className="text-xl font-semibold text-muted">{t('pageNotFound')}</p>
        <p className="text-muted">{t('notFoundDescription')}</p>
        <Link
          to="/"
          className="inline-block bg-secondary text-white px-6 py-2 rounded-full hover:bg-accent transition"
        >
          {t('goHome')}
        </Link>
      </div>
    </div>
  );
}

import type React from 'react';
import { useTranslation } from 'react-i18next';

const LanguageSwitcher = () => {
  const { i18n } = useTranslation();

  const changeLanguage = (lang: 'en' | 'fa', e: React.FormEvent) => {
    e.preventDefault();

    i18n.changeLanguage(lang);
    document.documentElement.dir = lang === 'fa' ? 'rtl' : 'ltr';
  };

  return (
    <div className="space-x-4 rtl:space-x-reverse">
      <button
        onClick={(e) => changeLanguage('en', e)}
        className={`text-sm ${i18n.language === 'en' ? 'font-bold text-background border-b-2' : 'text-accent'}`}
      >
        EN
      </button>
      <button
        onClick={(e) => changeLanguage('fa', e)}
        className={`text-sm ${i18n.language === 'fa' ? 'font-bold text-background border-b-2' : 'text-accent'}`}
      >
        فارسی
      </button>
    </div>
  );
};

export default LanguageSwitcher;

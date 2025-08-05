import { useRoutes } from 'react-router-dom';
import routes from './routes/routes';
import { useTranslation } from 'react-i18next';
import { useEffect } from 'react';
import './App.css'
import { ToastContainer } from 'react-toastify';


export default function App() {

  const { i18n } = useTranslation();

  useEffect(() => {
    if (i18n.language === 'fa') {
      document.documentElement.dir = 'rtl';
      document.documentElement.classList.add('rtl');
    } else {
      document.documentElement.dir = 'ltr';
      document.documentElement.classList.remove('rtl');
    }
  }, [i18n.language]);

  const routing = useRoutes(routes);

  return (
    <div className="min-h-screen bg-gray-50 font-sans">
      {/* {!location.pathname.includes('login') ? <Header /> : <></>} */}
      <main>{routing}</main>

        <ToastContainer
          position="top-right"
          autoClose={3000}   // 3 seconds auto close
          hideProgressBar={false}
          newestOnTop={false}
          closeOnClick
          rtl={false}  // Set true if you want RTL for Farsi
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme="light"
        />
    </div>
  );
}

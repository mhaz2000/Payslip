import { toast } from 'react-toastify';

export const displayError = (message: string) => {
  toast.error(message, {
    position: 'top-center',
    hideProgressBar: false,
    bodyClassName: 'text-black font-IranSans',
    theme: 'colored',
  });
};

export const displaySuccess = (message: string) => {
  toast.success(message, {
    position: 'top-center',
    hideProgressBar: false,
    bodyClassName: 'text-black font-IranSans',
    theme: 'colored',
  });
};
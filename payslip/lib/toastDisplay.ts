import { toast } from 'react-toastify';

export const displayError = (message: string) => {
  toast.error(message, {
    position: 'top-center',
    hideProgressBar: false,
    bodyClassName: 'text-black IranSansFaNumbers toast-style',
    theme: 'colored',
  });
};

export const displaySuccess = (message: string) => {
  toast.success(message, {
    position: 'top-center',
    hideProgressBar: false,
    bodyClassName: 'text-black IranSansFaNumbers toast-style',
    theme: 'colored',
  });
};
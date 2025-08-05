import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { loginUser, loginAdmin } from '../api/auth';
import type { LoginRequest, LoginResult } from '../types/auth';

export const useAuth = () => {
  const [isPending, setIsPending] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const login = async (credentials: LoginRequest, type: 'user' | 'admin' = 'user') => {
    setIsPending(true);
    setError(null);

    try {
      let result: LoginResult;

      if (type === 'admin') {
        result = await loginAdmin(credentials);
        localStorage.setItem('token', result.authToken);
        navigate('/admin/');
      } else {
        result = await loginUser(credentials);
        localStorage.setItem('token', result.authToken);
        navigate('/');
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'loginFailed');
    } finally {
      setIsPending(false);
    }
  };

  const logout = (type: 'user' | 'admin' = 'user') => {
    if (type === 'admin') {
      localStorage.removeItem('token');
      navigate('/admin/login');
    } else {
      localStorage.removeItem('token');
      navigate('/login');
    }
  };

  const isAuthenticated = {
    user: Boolean(localStorage.getItem('token')),
    admin: Boolean(localStorage.getItem('token')),
  };

  return {
    login,
    logout,
    isAuthenticated,
    isPending,
    error,
  };
};

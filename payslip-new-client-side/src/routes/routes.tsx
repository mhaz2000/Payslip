import type { RouteObject } from 'react-router-dom';
import ProtectedRoute from './ProtectedRoute';

import Login from '../pages/Login';
import NotFound from '../pages/NotFound'; // optional
import AdminLoginPage from '../pages/admin/AdminLogin';
import ProtectedAdminRoute from './ProtectedAdminRoute';
import { Users } from '../pages/admin/Users';
import AdminPersonalInfo from '../pages/admin/AdminPersonalInfo';
import { PayslipFiles } from '../pages/admin/PayslipFiles';
import UserPersonalInfo from '../pages/user/UserPersonalInfo';
import UserPayslipPage from '../pages/user/UserPayslipPage';
import Dashboard from '../pages/Dashboard';

const routes: RouteObject[] = [
  {
    path: '/login',
    element: <Login />,
  },
  {
    path: '/admin/login',
    element: <AdminLoginPage />,
  },
  {
    path: '/admin',
    element: <ProtectedAdminRoute />,
    children: [
      { index: true, element: <Dashboard /> },
      { path: 'users', element: <Users /> },
      { path: 'personalInfo', element: <AdminPersonalInfo /> },
      { path: 'payslips', element: <PayslipFiles /> },
    ]
  },
  {
    path: '/',
    element: <ProtectedRoute />,
    children: [
      { index: true, element: <Dashboard /> },
      { path: 'personalInfo', element: <UserPersonalInfo /> },
      { path: 'payslip', element: <UserPayslipPage /> },
    ],
  },
  {
    path: '*',
    element: <NotFound />, // optional
  },
];

export default routes;

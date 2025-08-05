import { Navigate, Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';

const ProtectedAdminRoute = () => {
  const token = localStorage.getItem('token');

  if (!token) {
    return <Navigate to="/admin/login" replace />;
  }

  return (
    <>
    <Sidebar>
      <Outlet />
    </Sidebar>
    </>
  );
};

export default ProtectedAdminRoute;

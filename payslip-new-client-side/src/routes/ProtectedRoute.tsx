import { Navigate, Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';

const ProtectedRoute = () => {
  const token = localStorage.getItem('token');

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  return (
    <Sidebar>
      <Outlet />
    </Sidebar>
  );
};

export default ProtectedRoute;
